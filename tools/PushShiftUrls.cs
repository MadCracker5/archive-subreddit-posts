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
        public static string GetSubredditPostsUrl(string subreddit, DateRange dateScope)
        {
            var beforeTimestamp = $"&before={DateRange.TotalSecondsFromEpoch(dateScope.Before)}&after={DateRange.TotalSecondsFromEpoch(dateScope.After)}";
            return string.Format("{0}/{1}/?size=500{2}{3}", _Base, _SubredditPostsUrl, $"&subreddit={subreddit}", beforeTimestamp);
        }
#nullable disable
        public static string GetCommentIdsUrl(string postId) =>
            $"{_Base}/{_CommentIds}/{postId}";
        public static string GetCommentsUrl(IEnumerable<string> commentIds) =>
            $"{_Base}/{_Comments}/?ids={string.Join(',', commentIds)}";
    }
}
