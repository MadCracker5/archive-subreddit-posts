using System;
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
            //var beforeTimestamp = $"&before={dateScope.Before}&after={dateScope.After}";
            var beforeTimestamp = $"&before={dateScope}";
            var tt = string.Format("{0}/{1}/?size=100{2}{3}", _Base, _SubredditPostsUrl, $"&subreddit={subreddit}", beforeTimestamp);
            return tt;
        }
#nullable disable
        public static string GetCommentIdsUrl(string postId) =>
            $"{_Base}/{_CommentIds}/{postId}";
        public static string GetCommentsUrl(IEnumerable<string> commentIds) =>
            $"{_Base}/{_Comments}/?ids={string.Join(',', commentIds)}";
    }
} // https://api.pushshift.io/reddit/search/submission/?size=100&subreddit=funny&before=1594598441&after=1594598400
                                                                                         