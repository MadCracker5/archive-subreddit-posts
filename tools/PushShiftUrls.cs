using System.Collections.Generic;

namespace reddit_scraper.Tools
{
    public static class PushShiftApiUrls
    {
        private const string _Base = "https://api.pushshift.io/reddit";
        private const string _SubredditPostsUrl = "search/submission";
        private const string _Comments = "search/comment";
        private const string _CommentIds = "submission/comment_ids";
#nullable enable
        public static string GetSubredditPostsUrl(string subreddit, long dateScope)
        {
            var tt = string.Format("{0}/{1}/?size=500&limit=1000&sort=desc{2}{3}", _Base, _SubredditPostsUrl, $"&subreddit={subreddit}", $"&before={dateScope}");
            return tt;
        }
#nullable disable
        public static string GetCommentIdsUrl(string postId) =>
            $"{_Base}/{_CommentIds}/{postId}";
        public static string GetCommentsUrl(IEnumerable<string> commentIds) =>
            $"{_Base}/{_Comments}/?ids={string.Join(',', commentIds)}";
    }
}