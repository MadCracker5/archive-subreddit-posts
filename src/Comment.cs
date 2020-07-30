using System;

namespace reddit_scraper
{
    public partial class Comment
    {
        public object[] AllAwardings { get; set; }
        public bool AllowLiveComments { get; set; }
        public string Author { get; set; }
        public object AuthorFlairCssClass { get; set; }
        public object[] AuthorFlairRichtext { get; set; }
        public object AuthorFlairText { get; set; }
        public string AuthorFlairType { get; set; }
        public string AuthorFullname { get; set; }
        public bool AuthorPatreonFlair { get; set; }
        public bool AuthorPremium { get; set; }
        public object[] Awarders { get; set; }
        public bool CanModPost { get; set; }
        public bool ContestMode { get; set; }
        public long CreatedUtc { get; set; }
        public string Domain { get; set; }
        public Uri FullLink { get; set; }
        public Gildings Gildings { get; set; }
        public string Id { get; set; }
        public bool IsCrosspostable { get; set; }
        public bool IsMeta { get; set; }
        public bool IsOriginalContent { get; set; }
        public bool IsRedditMediaDomain { get; set; }
        public bool IsRobotIndexable { get; set; }
        public bool IsSelf { get; set; }
        public bool IsVideo { get; set; }
        public string LinkFlairBackgroundColor { get; set; }
        public object[] LinkFlairRichtext { get; set; }
        public string LinkFlairTextColor { get; set; }
        public string LinkFlairType { get; set; }
        public bool Locked { get; set; }
        public bool MediaOnly { get; set; }
        public bool NoFollow { get; set; }
        public long NumComments { get; set; }
        public long NumCrossposts { get; set; }
        public bool Over18 { get; set; }
        public string Permalink { get; set; }
        public bool Pinned { get; set; }
        public long RetrievedOn { get; set; }
        public long Score { get; set; }
        public string Selftext { get; set; }
        public bool SendReplies { get; set; }
        public bool Spoiler { get; set; }
        public bool Stickied { get; set; }
        public string Subreddit { get; set; }
        public string SubredditId { get; set; }
        public long SubredditSubscribers { get; set; }
        public string SubredditType { get; set; }
        public string Thumbnail { get; set; }
        public string Title { get; set; }
        public long TotalAwardsReceived { get; set; }
        public object[] TreatmentTags { get; set; }
        public long UpvoteRatio { get; set; }
        public Uri Url { get; set; }
    }

    public partial class Gildings
    {
    }
}
