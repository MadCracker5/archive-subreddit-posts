using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using reddit_scraper.DataHolders;
using reddit_scraper.DataHolders.CommentResponseParser;
using reddit_scraper.DataHolders.PostResponseParser;
using reddit_scraper.Http;
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
        public PostArchiver(IServiceProvider provider)
        {
            var config = provider.GetService<IConfigurationRoot>();
            _subreddit_target = config.GetSection("subreddit").Value;
            _output_directory = config.GetSection("out_directory").Value;
            _serviceProvider = provider;
        }
#nullable enable
        async Task<IEnumerable<Post>?> GetSubredditPostsAsync(DateRange dateScope)
        {
            var url = PushShiftApiUrls.GetSubredditPostsUrl(_subreddit_target, dateScope);
            try {
                var jsonString = await _serviceProvider
                    .GetRequiredService<IHttpClientThrottler>()
                    .MakeRequestAsync(url);
                return PostResponse.FromJson(jsonString).Posts;
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
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
                Console.WriteLine(e.ToString());
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
                Console.WriteLine(e.ToString());
                return null;
            }
        }
        async Task<IEnumerable<UnresolvedPostArhive>> ResolveCommentIds(IEnumerable<Post> posts)
        {
            var commentIdsTasks = new List<Task<UnresolvedPostArhive?>>();
            foreach (var post in posts) {
                commentIdsTasks.Add(GetCommentIdsAsync(post));
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
                return new PostArchive
                {
                    Post = postArchive.Post,
                    Comments = await GetCommentsAsync(postArchive.CommentIds)
                };
            }
            var chopper = postLength / 270;
            var cutOff = postLength / chopper;
            var commentTasks = new List<Task<Comment[]?>>();
            for (var i = 0; i < chopper; i++) {
                var cur_sel = postArchive.CommentIds.Skip(i * cutOff).Take((i + 1) * cutOff);
                commentTasks.Add(GetCommentsAsync(cur_sel.ToArray()));
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
            var posts = await GetSubredditPostsAsync(dateScope);
            if (posts == null) {
                return null;
            }
            var unresolvedPostArhives = await ResolveCommentIds(posts);
            var postArchiveTasks = new List<Task<PostArchive>>();
            foreach (var unresolvedPostArhive in unresolvedPostArhives) {
                postArchiveTasks.Add(ResolveComments(unresolvedPostArhive));
            }
            return await Task.WhenAll(postArchiveTasks.ToArray());
        }
        async Task GetPostArchivesInRange(DateRange dateScope)
        {
            var postArchives = new List<PostArchive>();
            var currentPostArchives = await GetPostArchives(dateScope);
            while (currentPostArchives != null) {
                postArchives.AddRange(currentPostArchives);
                var nextCutoff = currentPostArchives.OrderBy(x => x.Post.CreatedUtc).FirstOrDefault().Post.CreatedUtc;
                if (nextCutoff < DateRange.TotalSecondsFromEpoch(dateScope.After)) {
                    break;
                }
                dateScope = new DateRange
                {
                    After = DateRange.UnixTimeStampToDateTime((double)nextCutoff),
                    Before = dateScope.Before
                };
                currentPostArchives = await GetPostArchives(dateScope);
            }
            var serializedPostArchive = JsonConvert.SerializeObject(new Dictionary<string, List<PostArchive>> { ["posts"] = postArchives });
            var fn = $"{_output_directory}/{dateScope.After.ToShortDateString()}.zip";
            File.WriteAllText(fn, serializedPostArchive);
            Console.WriteLine($"Wrote {postArchives.Count()} Archives to {fn}");
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
            var i = 0;
            var postArchiveTasks = new List<Task>();
            foreach (var date in dates) {
                postArchiveTasks.Add(GetPostArchivesInRange(date));
                if (i % 5 == 0 && i != 0) {
                    await Task.WhenAll(postArchiveTasks.ToArray());
                    postArchiveTasks = new List<Task>();
                }
                i++;
            }
            await Task.WhenAll(postArchiveTasks.ToArray());
        }
        void PrintDefaultInfo(DateTime after, DateTime before)
        {
            var firstLine = $"Parsing posts & comments for subreddit - {_subreddit_target} after {after.ToShortDateString()} and before {before.ToShortDateString()}";
            var secondLine = $"Files will be written to {AppDomain.CurrentDomain.BaseDirectory}{_output_directory}";
            var stars = string.Join("", Enumerable.Repeat("*", firstLine.Length));
            Console.WriteLine($"{stars}\n{firstLine}\n{secondLine}\n{stars}\n\n");
        }
#nullable disable
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
            return date_list;
        }
        public void Run()
        {
            var dateRanges = BuildDateRanges();
            GetSubredditArchive(dateRanges).GetAwaiter().GetResult();
        }
    }
}
