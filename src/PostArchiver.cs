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
        private readonly Interval _interval;
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

            _interval = DateConfig.ParseInterval(config);
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
            var postsWithComments = unresolvedPostArchives.Where(x => x.CommentIds != null && x.CommentIds.Any()).Select(x => x.CommentIds.Count());
            var numComments = postsWithComments.Any()
                ? postsWithComments.Aggregate((a, b) => a + b)
                : 0;
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
        async Task GetPostArchivesInRange(DayDateRange dateRanges)
        {
            var postArchives = new List<PostArchive>();
            var numIters = 0;
            foreach (var dateRange in dateRanges.Intervals) {
                NextSubredditDetails(dateRange, numIters);
                var currentPostArchives = await GetPostArchives(dateRange);
                numIters++;
                if (currentPostArchives != null) {
                    postArchives.AddRange(currentPostArchives);
                }
            }
            var serializedPostArchive = JsonConvert.SerializeObject(new Dictionary<string, List<PostArchive>> { ["posts"] = postArchives });
            var fn = $"{_output_directory}/{dateRanges.Date.ToShortDateString()}.json";
            File.WriteAllText(fn, serializedPostArchive);
            Console.WriteLine($"\n\nWrote {postArchives.Count()} Archives to {fn}");
        }
        /// <summary>
        /// For each date range in our iterable of date ranges, process up to 5 concurrently into archives corresponding to the day they are.
        /// </summary>
        /// <param name="dates"></param>
        /// <returns></returns>
        async Task GetSubredditArchive(IEnumerable<DayDateRange> dayRanges)
        {
            if (!Directory.Exists(_output_directory)) {
                Directory.CreateDirectory(_output_directory);
            }
            _total_dates = dayRanges.Count();
            _current_date_idx = 0;
            foreach (var dayRange in dayRanges) {
                _current_comment_total = 0;
                _current_post_total = 0;
                await GetPostArchivesInRange(dayRange);
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
        /// Divide up our operations into days and days into the specified time intervals so that each output file is delineated by days & requests delineated by the specified interval.
        /// </summary>
        /// <returns></returns>
        IEnumerable<DayDateRange> BuildDateRanges()
        {
            var config = _serviceProvider.GetService<IConfigurationRoot>();
            var before = DateConfig.ParseDateCutoffSection(config, DateConfigEnum.Before);
            var after = DateConfig.ParseDateCutoffSection(config, DateConfigEnum.After);
            PrintDefaultInfo(after, before);
            var cutoff = after;
            var now = before;
            var total_days = (now - cutoff).TotalDays;
            var dayDateRange = new List<DayDateRange>();
            for (var _ = 0; _ < total_days; _++) {
                now = now.AddDays(-1);
                var dateList = new List<DateRange>();
                for (var y = 0; y < 48; y++) {
                    now = _interval.GetNextTimestep(now, y);
                    dateList.Add(new DateRange
                    {
                        After = DateRange.TotalSecondsFromEpoch(now),
                        Before = _interval.OffsetBeforeFn(now, y)
                    });
                }
                dayDateRange.Add(new DayDateRange { 
                    Date = now,
                    Intervals = dateList
                });
            }
            return dayDateRange;
        }
        void NextSubredditDetails(DateRange dateScope, int numIters = 0)
        {
            Console.Clear();
            Console.WriteLine(_get_init_string(_current_date_idx, _total_dates));
            Console.WriteLine($"\n{string.Join("", Enumerable.Repeat("/", 44))}\n");
            Console.Write("Date: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateRange.UnixTimeStampToDateTime(dateScope.Before).ToShortDateString()}");
            Console.ResetColor();
            Console.Write($"Round({_interval.Value} {_interval.Type} per): ");
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
