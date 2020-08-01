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
        private Func<int, int, string> _get_init_string;
        private readonly bool _verbosity;
        private readonly string _subreddit_target;
        private readonly string _output_directory;
        private readonly IServiceProvider _serviceProvider;
        private int _current_comment_total;
        private int _current_post_total;
        private int _current_date_idx;
        private int _total_dates;
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
        async Task<Post[]?> GetSubredditPostsAsync(DateRange dateScope)
        {
            var url = PushShiftApiUrls.GetSubredditPostsUrl(_subreddit_target, dateScope);
            try {
                var jsonString = await _serviceProvider
                    .GetRequiredService<IHttpClientThrottler>()
                    .MakeRequestAsync(url);
                return PostResponse.FromJson(jsonString).Posts;
            } catch (Exception e) {
                if (_verbosity) Console.WriteLine(e.ToString());
                return null;
            }
        }
        async Task<UnresolvedPostArhive?> GetCommentIdsAsync(Post post)
        {
            var url = PushShiftApiUrls.GetCommentIdsUrl(post.Id);
            try {
                var jsonString = await _serviceProvider
                    .GetRequiredService<IHttpClientThrottler>()
                    .MakeRequestAsync(url);
                return new UnresolvedPostArhive
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
        async Task<IEnumerable<UnresolvedPostArhive>> ResolveCommentIds(Post[] posts)
        {
            var commentIdsTasks = new List<Task<UnresolvedPostArhive?>>();
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
            var postArchivesNotNull = new List<UnresolvedPostArhive>();
            foreach (var postArchive in postArchives) {
                if (postArchive == null) {
                    continue;
                }
                postArchivesNotNull.Add(postArchive);
            }
            return postArchivesNotNull;
        }
        async Task<PostArchive> ResolveComments(UnresolvedPostArhive postArchive)
        {
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

        async Task<IEnumerable<PostArchive>?> GetPostArchives(DateRange dateScope)
        {
            Console.Write($"\nSearching for posts...\t");
            var posts = await GetSubredditPostsAsync(dateScope);
            if (posts == null || posts.Length == 0) {
                return null;
            }
            _current_post_total += posts.Length;
            Console.Write($"{posts.Length} posts found.\n");
            Console.Write($"\nFinding comment ids for posts...\t");
            var unresolvedPostArchives = await ResolveCommentIds(posts);
            var numComments = unresolvedPostArchives.Where(x => x.CommentIds.Any()).Select(x => x.CommentIds.Count()).Aggregate((a, b) => a + b);
            _current_comment_total += numComments;
            Console.WriteLine($"\n{numComments} total comment ids found in {posts.Length} posts.");
            var numUnresolved = unresolvedPostArchives.Count();
            var postArchiveTasks = new List<Task<PostArchive>>();
            var numCompleted = 0;
            using var progress = new ProgressBar();
            Console.Write("\nComment ids -> Comments...\t");
            foreach (var unresolvedPostArchive in unresolvedPostArchives) {
                var task = ResolveComments(unresolvedPostArchive);
                _ = task.ContinueWith(s =>
                {
                    numCompleted++;
                    progress.Report((float)numCompleted / (float)posts.Length);
                });
                postArchiveTasks.Add(task);
            }
            var results = await Task.WhenAll(postArchiveTasks.ToArray());
            Console.WriteLine($"\nFinished with {numUnresolved} posts.");
            return results;
        }
        async Task GetPostArchivesInRange(DateRange dateScope)
        {
            var postArchives = new List<PostArchive>();
            var currentPostArchives = await GetPostArchives(dateScope);
            var numIters = 0;
            while (currentPostArchives != null) {
                numIters++;
                postArchives.AddRange(currentPostArchives);
                var nextCutoff = currentPostArchives.OrderBy(x => x.Post.CreatedUtc).FirstOrDefault().Post.CreatedUtc;
                if (nextCutoff < DateRange.TotalSecondsFromEpoch(dateScope.After)) {
                    break;
                }
                NextSubredditDetails(dateScope, numIters);
                dateScope = new DateRange
                {
                    After = dateScope.After,
                    Before = DateRange.UnixTimeStampToDateTime((double)nextCutoff)
                };
                currentPostArchives = await GetPostArchives(dateScope);
            }
            var serializedPostArchive = JsonConvert.SerializeObject(new Dictionary<string, List<PostArchive>> { ["posts"] = postArchives });
            var fn = $"{_output_directory}/{dateScope.After.ToShortDateString()}.json";
            File.WriteAllText(fn, serializedPostArchive);
            Console.WriteLine($"\n\nWrote {postArchives.Count()} Archives to {fn}");
        }
        /// <summary>
        /// For each date range in our iterable of date ranges, process up to 5 concurrently into archives corresponding to the day they are.
        /// </summary>
        /// <param name="dates"></param>
        /// <returns></returns>
        async Task GetSubredditArchive(IEnumerable<DateRange> dates)
        {
            if (!Directory.Exists(_output_directory)) {
                Directory.CreateDirectory(_output_directory);
            }
            _total_dates = dates.Count();
            _current_date_idx = 0;
            foreach (var date in dates) {
                _current_comment_total = 0;
                _current_post_total = 0;
                NextSubredditDetails(date);
                await GetPostArchivesInRange(date);
                _current_date_idx++;
            }
        }
        void PrintDefaultInfo(DateTime after, DateTime before)
        {
            var firstLine = $"Parsing posts & comments for subreddit - {_subreddit_target} after {after.ToShortDateString()} and before {before.ToShortDateString()}";
            var secondLine = $"Files will be written to {AppDomain.CurrentDomain.BaseDirectory}{_output_directory}";
            var stars = string.Join("", Enumerable.Repeat("*", firstLine.Length));
            _get_init_string = (Func<int, int, string>)((int dateIdx, int totalDates) => $"{stars}\n{firstLine}\n{secondLine}\n{stars}\n\nDate: {dateIdx}/{totalDates}");
        }
#nullable disable
        public void Run()
        {
            var dateRanges = BuildDateRanges();
            GetSubredditArchive(dateRanges).GetAwaiter().GetResult();
        }
        /// <summary>
        /// Divide up our operations into days so that each output file is delineated by days.
        /// </summary>
        /// <returns></returns>
        IEnumerable<DateRange> BuildDateRanges()
        {
            var config = _serviceProvider.GetService<IConfigurationRoot>();
            var before = DateConfig.ParseSection(config.GetSection("before"), DateConfigEnum.Before);
            var after = DateConfig.ParseSection(config.GetSection("after"), DateConfigEnum.After);
            PrintDefaultInfo(after, before);
            var cutoff = after;
            var now = before;
            var total_days = (now - cutoff).TotalDays;
            var date_list = new List<DateRange>();
            for (var _ = 0; _ < total_days; _++) {
                now = now.AddDays(-1);
                date_list.Add(new DateRange
                {
                    After = now,
                    Before = now.AddSeconds(86399)
                });
            }
            return date_list.OrderByDescending(x => x.Before);
        }
        void NextSubredditDetails(DateRange dateScope, int numIters = 0)
        {
            Console.Clear();
            Console.WriteLine(_get_init_string(_current_date_idx, _total_dates));
            Console.WriteLine($"\n{string.Join("", Enumerable.Repeat("/", 44))}\n");
            Console.Write("Date: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{dateScope.Before.ToShortDateString()}");
            Console.ResetColor();
            Console.Write("Round: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"#{numIters}");
            Console.ResetColor();
            Console.Write($"Posts: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{_current_post_total}");
            Console.ResetColor();
            Console.Write($"Comments: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{_current_comment_total}");
            Console.ResetColor();
            Console.WriteLine($"\n{string.Join("", Enumerable.Repeat("/", 44))}\n");
        }
    }
}
