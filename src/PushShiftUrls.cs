using System.Collections.Generic;

namespace reddit_scraper
{
    public static class PushShiftApiUrls
    {
        private const string _Base = "https://api.pushshift.io/reddit";
        private const string _SubredditPostsUrl = "search/submission";
        private const string _Comments = "search/comment";
        private const string _CommentIds = "submission/comment_ids";
#nullable enable
        public static string GetSubredditPostsUrl(string subreddit, string limit, DateRange dateScope)
        {
            var beforeTimestamp = $"&before={DateRange.TotalSecondsFromEpoch(dateScope.End)}&after={DateRange.TotalSecondsFromEpoch(dateScope.Start)}";
            return string.Format("{0}/{1}/{2}{3}{4}", _Base, _SubredditPostsUrl, $"?subreddit={subreddit}", $"&limit={limit}", beforeTimestamp);
        }
#nullable disable
        public static string GetCommentIdsUrl(string postId) =>
            $"{_Base}/{_CommentIds}/{postId}";
        public static string GetCommentsUrl(IEnumerable<string> commentIds) =>
            $"{_Base}/{_Comments}/?ids={string.Join(',', commentIds)}";
    }
}
