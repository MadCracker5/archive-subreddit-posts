using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using reddit_scraper.DataHolders;
using reddit_scraper.DataHolders.CommentResponseParser;
using reddit_scraper.DataHolders.PostResponseParser;
using reddit_scraper.Http;
using reddit_scraper.tools;
using reddit_scraper.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace reddit_scraper.Src
{
    public interface IPostArchiver
    {
        public void Run();
    }
    public class PostArchiver : IPostArchiver
    {
        private readonly string _subreddit_target;
        private readonly string _output_directory;
        private readonly IServiceProvider _serviceProvider;
        private readonly bool _verbosity;
        private long NextCreatedUtc;
        private long Before;
        private long After;
        private int TotalDays;
        private int CurrentDateIdx;
        private Func<int, int, string> GetInitString;
        public PostArchiver(IServiceProvider provider)
        {
            var config = provider.GetService<IConfigurationRoot>();
            _subreddit_target = config.GetSection("subreddit").Value;
            _output_directory = config.GetSection("out_directory").Value;
            _serviceProvider = provider;
            var verbosity = config.GetSection("verbosity").Value;
            if (int.TryParse(verbosity, out int verbosityInt)) {
                _verbosity = verbosityInt != 0;
            }
        }
#nullable enable
        async Task<PostResponse?> GetSubredditPostsAsync()
        {
            var url = PushShiftApiUrls.GetSubredditPostsUrl(_subreddit_target, NextCreatedUtc);
            try {
                var jsonString = await _serviceProvider
                    .GetRequiredService<IHttpClientThrottler>()
                    .MakeRequestAsync(url);
                return PostResponse.FromJson(jsonString);
            } catch (Exception e) {
                if (_verbosity) Console.WriteLine(e.ToString());
                return null;
            }
        }
        async Task<UnresolvedPostArchive?> GetCommentIdsAsync(Post post)
        {
            var url = PushShiftApiUrls.GetCommentIdsUrl(post.Id);
            try {
                var jsonString = await _serviceProvider
                    .GetRequiredService<IHttpClientThrottler>()
                    .MakeRequestAsync(url);
                return new UnresolvedPostArchive
                {
                    Post = post,
                    CommentIds = JsonConvert.DeserializeObject<PushshiftResponse<string>>(jsonString).Data
                };
            } catch (Exception e) {
                if (_verbosity) Console.WriteLine(e.ToString());
                return null;
            }
        }
        async Task<Comment[]?> GetCommentsAsync(IEnumerable<string> commentIds)
        {
            var url = PushShiftApiUrls.GetCommentsUrl(commentIds);
            try {
                var jsonString = await _serviceProvider
                    .GetRequiredService<IHttpClientThrottler>()
                    .MakeRequestAsync(url);
                return CommentResponse.FromJson(jsonString).Comments;
            } catch (Exception e) {
                if (_verbosity) Console.WriteLine(e.ToString());
                return null;
            }
        }
        async Task<IEnumerable<UnresolvedPostArchive>> ResolveCommentIds(Post[] posts)
        {
            //return posts.Select(x => new UnresolvedPostArchive { Post = x, CommentIds = new string[] { } });
            var commentIdsTasks = new List<Task<UnresolvedPostArchive?>>();
            var numCompleted = 0;
            using var progress = new ProgressBar();
            foreach (var post in posts) {
                var task = GetCommentIdsAsync(post);
                _ = task.ContinueWith(_ =>
                {
                    numCompleted++;
                    progress.Report((float)numCompleted / (float)posts.Length);
                });
                commentIdsTasks.Add(task);
            }
            var postArchives = await Task.WhenAll(commentIdsTasks.ToArray());
            var postArchivesNotNull = new List<UnresolvedPostArchive>();
            foreach (var postArchive in postArchives) {
                if (postArchive == null) {
                    continue;
                }
                postArchivesNotNull.Add(postArchive);
            }
            return postArchivesNotNull;
        }
        async Task<PostArchive> ResolveComments(UnresolvedPostArchive postArchive)
        {
            //return new PostArchive { Post = postArchive.Post, Comments = new Comment[] { } };
            var postLength = postArchive.CommentIds.Count();
            if (postLength == 0) {
                return new PostArchive { Post = postArchive.Post };
            }
            if (postLength < 273) {
                var earlyComments = await GetCommentsAsync(postArchive.CommentIds);
                return new PostArchive
                {
                    Post = postArchive.Post,
                    Comments = earlyComments ?? Enumerable.Empty<Comment>()
                };
            }
            var chopper = (float)postLength / 270f;
            var cutOff = (int)Math.Round((float)postLength / chopper);
            var commentTasks = new List<Task<Comment[]?>>();
            for (var i = 0; i < chopper; i++) {
                var cur_sel = postArchive.CommentIds.Skip(i * cutOff).Take((i + 1) * cutOff);
                var task = GetCommentsAsync(cur_sel.ToArray());
                commentTasks.Add(task);
            }
            var comments = await Task.WhenAll(commentTasks.ToArray());
            return new PostArchive
            {
                Post = postArchive.Post,
                Comments = comments.SelectMany(x => x),
            };
        }
        async Task<PostArchive[]> GetPostArchivesFromPosts(Post[] posts)
        {
            var unresolvedPostArchives = await ResolveCommentIds(posts);
            var postsWithComments = unresolvedPostArchives.Where(x => x.CommentIds != null && x.CommentIds.Any()).Select(x => x.CommentIds.Count());
            var numComments = postsWithComments.Any()
                ? postsWithComments.Aggregate((a, b) => a + b)
                : 0;
            Console.WriteLine($"\n{numComments} total comment ids found in {posts.Length} posts.");
            var numUnresolved = unresolvedPostArchives.Count();
            var postArchiveTasks = new List<Task<PostArchive>>();
            var numCompleted = 0;
            using var progress = new ProgressBar();
            Console.Write("\nComment Ids -> Comments...\t");
            foreach (var unresolvedPostArchive in unresolvedPostArchives) {
                var task = ResolveComments(unresolvedPostArchive);
                _ = task.ContinueWith(s =>
                {
                    numCompleted++;
                    progress.Report((float)numCompleted / (float)posts.Length);
                });
                postArchiveTasks.Add(task);
            }
            return await Task.WhenAll(postArchiveTasks.ToArray());
        }
        async Task GetPostArchives()
        {
            var postArchives = new List<PostArchive>();
            var currentDate = DateRange.UnixTimeStampToDateTime(NextCreatedUtc).Date;
            while (NextCreatedUtc > After) {
                Console.Write($"\nSearching for posts...\t");
                var postResponse = await GetSubredditPostsAsync();
                if (postResponse == null || postResponse.Posts.Length == 0) {
                    continue;
                }
                Console.Write($"{postResponse.Posts.Length} posts found.\n");
                Console.Write($"\nFinding comment ids for posts...\t");
                var results = await GetPostArchivesFromPosts(postResponse.Posts);
                postArchives.AddRange(results);
                Console.WriteLine($"\nFinished with {postResponse.Posts.Length} posts.");
                NextCreatedUtc = (long)postResponse.Posts.LastOrDefault().CreatedUtc + 1;
                var nextUtcDate = DateRange.UnixTimeStampToDateTime(NextCreatedUtc);
                NextSubredditDetails(DateRange.TotalSecondsFromEpoch(currentDate));
                if (currentDate.DayOfYear != nextUtcDate.DayOfYear) {
                    // TODO: Resolve issue where day doesn't have many posts and the last result of the return value is > 1 days difference with the previous day.
                    var postArchivesOfDay = postArchives.Where(x => DateRange.UnixTimeStampToDateTime(x.Post.CreatedUtc).DayOfYear == currentDate.DayOfYear).Distinct().Select(x => x).ToList();
                    var numPostArchivesofDay = postArchivesOfDay.Count();
                    var postsWithCommentsOfDay = postArchivesOfDay.Where(x => x.Comments != null && x.Comments.Any()).Select(x => x.Comments.Count());
                    var numComments = postsWithCommentsOfDay.Any()
                        ? postsWithCommentsOfDay.Aggregate((a, b) => a + b)
                        : 0;
                    var serializedPostArchive = JsonConvert.SerializeObject(new Dictionary<string, List<PostArchive>> { ["posts"] = postArchivesOfDay });
                    var fn = $"{_output_directory}/{currentDate.ToShortDateString()}.json";
                    File.WriteAllText(fn, serializedPostArchive);
                    Console.WriteLine($"\n\nWrote {numPostArchivesofDay} posts and {numComments} comments in an archive to {fn}");
                    postArchives = postArchives.Where(x => DateRange.UnixTimeStampToDateTime(x.Post.CreatedUtc).DayOfYear != currentDate.DayOfYear).Select(x => x).ToList();
                    CurrentDateIdx++;
                    currentDate = nextUtcDate.Date;
                    NextSubredditDetails(DateRange.TotalSecondsFromEpoch(currentDate));
                }
            }

        }

#nullable disable
        void BuildDateRanges()
        {
            var config = _serviceProvider.GetService<IConfigurationRoot>();
            var beforeDate = DateConfig.ParseDateCutoffSection(config, DateConfigEnum.Before).AddDays(-1).AddSeconds(86399);
            var afterDate = DateConfig.ParseDateCutoffSection(config, DateConfigEnum.After);
            Before = DateRange.TotalSecondsFromEpoch(beforeDate);
            After = DateRange.TotalSecondsFromEpoch(afterDate);
            NextCreatedUtc = Before;
            CurrentDateIdx = 1;
            TotalDays = (int)(beforeDate - afterDate).TotalDays;
        }
        public void Run()
        {
            BuildDateRanges();
            if (!Directory.Exists(_output_directory)) {
                Directory.CreateDirectory(_output_directory);
            }
            PrintDefaultInfo();
            NextSubredditDetails();
            GetPostArchives().GetAwaiter().GetResult();
        }
        void PrintDefaultInfo()
        {
            var firstLine = $"Parsing posts & comments for subreddit - {_subreddit_target} after {DateRange.UnixTimeStampToDateTime(After).ToShortDateString()} and before {DateRange.UnixTimeStampToDateTime(Before).ToShortDateString()}";
            var secondLine = $"Files will be written to {AppDomain.CurrentDomain.BaseDirectory}{_output_directory}";
            var stars = string.Join("", Enumerable.Repeat("*", firstLine.Length));
            GetInitString = (Func<int, int, string>)((int dateIdx, int totalDates) => $"{stars}\n{firstLine}\n{secondLine}\n{stars}\n\nDate: {dateIdx}/{totalDates}");
        }
        void NextSubredditDetails(long currentDate = 0)
        {
            Console.Clear();
            Console.WriteLine(GetInitString(CurrentDateIdx, TotalDays));
            Console.WriteLine($"\n{string.Join("", Enumerable.Repeat("/", 44))}\n");
            Console.Write("Date: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateRange.UnixTimeStampToDateTime(currentDate == 0 ? NextCreatedUtc : currentDate).ToShortDateString()}");
            Console.ResetColor();
            Console.WriteLine($"\n{string.Join("", Enumerable.Repeat("/", 44))}\n");
        }
    }
}
