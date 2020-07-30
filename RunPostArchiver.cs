using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace reddit_scraper
{
    public class RunPostArchiver
    {
        public static IConfigurationRoot configuration;
        private string _subreddit_target;
        private string _limit_per_request;
        private string _output_directory;
        async Task<string?> Get(string url)
        {
            using var client = new HttpClient();
            try {
                return await client.GetStringAsync(url);
            } catch (HttpRequestException e) {
                Console.WriteLine("Message :{0} ", e.Message);
            }
            return null;
        }
#nullable enable
        async Task<IEnumerable<Post>?> GetSubredditPostsAsync(DateRange dateScope)
        {
            var url = PushShiftApiUrls.GetSubredditPostsUrl(_subreddit_target, _limit_per_request, dateScope);
            var res = await Get(url);
            if (res == null) {
                return null;
            }
            try {
                return JsonConvert.DeserializeObject<IEnumerable<Post>>(res);
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
        async Task<UnresolvedPostArhive?> GetCommentIdsAsync(Post post)
        {
            var url = PushShiftApiUrls.GetCommentIdsUrl(post.Id);
            var res = await Get(url);
            if (res == null) {
                return null;
            }
            try {
                return new UnresolvedPostArhive
                {
                    Post = post,
                    CommentIds = JsonConvert.DeserializeObject<string[]>(res)
                };
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
        async Task<Comment[]?> GetCommentsAsync(IEnumerable<string> commentIds)
        {
            var url = PushShiftApiUrls.GetCommentsUrl(commentIds);
            var res = await Get(url);
            if (res == null) {
                return null;
            }
            try {
                return JsonConvert.DeserializeObject<Comment[]>(res);
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
                var nextCutoff = currentPostArchives.OrderByDescending(x => x.Post.CreatedUtc).FirstOrDefault().Post.CreatedUtc;
                dateScope = new DateRange
                {
                    Start = DateRange.UnixTimeStampToDateTime(nextCutoff),
                    End = dateScope.End
                };
                currentPostArchives = await GetPostArchives(dateScope);
            }
            var serializedPostArchive = JsonConvert.SerializeObject(new Dictionary<string, List<PostArchive>> { ["posts"] = postArchives });
            var fn = $"{_output_directory}/{dateScope.Start.ToShortDateString()}.json";
            File.WriteAllText(fn, serializedPostArchive);
            Console.WriteLine($"Wrote {postArchives.Count()} Archives to {fn}");
        }

        async Task GetSubredditArchive(IEnumerable<DateRange> dates)
        {
            _subreddit_target = configuration.GetSection("subreddit").Value;
            _limit_per_request = configuration.GetSection("limit").Value;
            _output_directory = configuration.GetSection("out_directory").Value;
            if (!Directory.Exists(_output_directory)) {
                Directory.CreateDirectory(_output_directory);
            }
            var postArchiveTasks = new List<Task>();
            foreach (var date in dates) {
                postArchiveTasks.Add(GetPostArchivesInRange(date));
            }
            await Task.WhenAll(postArchiveTasks.ToArray());
        }
#nullable disable
        static IEnumerable<DateRange> BuildUtcs()
        {
            DateTime.Today.AddSeconds(86399);
            var cutoff = new DateTime(2020, 07, 27);
            var now = DateTime.Today;
            var total_days = (now - cutoff).TotalDays;
            var utc_list = new List<DateRange>();
            for (var i = 0; i < total_days; i++) {
                now = now.AddDays(-i);
                utc_list.Add(new DateRange
                {
                    Start = now,
                    End = now.AddSeconds(86399)
                });
            }
            return utc_list;
        }
        public void Run()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var dateRanges = BuildUtcs();
            GetSubredditArchive(dateRanges).GetAwaiter().GetResult();
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();
            serviceCollection.AddSingleton(configuration);
        }
    }
}
