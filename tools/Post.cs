using System;

namespace reddit_scraper
{

    public partial class Post
    {
        public object[] AllAwardings { get; set; }
        public bool AllowLiveComments { get; set; }
        public string Author { get; set; }
        public object AuthorFlairCssClass { get; set; }
        public AuthorFlairRichtext[] AuthorFlairRichtext { get; set; }
        public string AuthorFlairText { get; set; }
        public AuthorFlairType? AuthorFlairType { get; set; }
        public string AuthorFullname { get; set; }
        public bool? AuthorPatreonFlair { get; set; }
        public bool? AuthorPremium { get; set; }
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
        public FlairTextColor LinkFlairTextColor { get; set; }
        public AuthorFlairType LinkFlairType { get; set; }
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
        public PostEnum Subreddit { get; set; }
        public SubredditId SubredditId { get; set; }
        public long SubredditSubscribers { get; set; }
        public SubredditType SubredditType { get; set; }
        public ThumbnailUnion Thumbnail { get; set; }
        public string Title { get; set; }
        public long TotalAwardsReceived { get; set; }
        public object[] TreatmentTags { get; set; }
        public long UpvoteRatio { get; set; }
        public Uri Url { get; set; }
        public string AuthorFlairBackgroundColor { get; set; }
        public FlairTextColor? AuthorFlairTextColor { get; set; }
        public PostHint? PostHint { get; set; }
        public Preview Preview { get; set; }
        public Guid? AuthorFlairTemplateId { get; set; }
        public long? ThumbnailHeight { get; set; }
        public long? ThumbnailWidth { get; set; }
        public Uri UrlOverriddenByDest { get; set; }
        public string CrosspostParent { get; set; }
        public CrosspostParentList[] CrosspostParentList { get; set; }
        public string RemovedByCategory { get; set; }
        public RedditPostMedia Media { get; set; }
        public MediaEmbed MediaEmbed { get; set; }
        public RedditPostMedia SecureMedia { get; set; }
        public MediaEmbed SecureMediaEmbed { get; set; }
    }

    public partial class AuthorFlairRichtext
    {
        public AuthorFlairType E { get; set; }
        public string T { get; set; }
    }

    public partial class CrosspostParentList
    {
        public object[] AllAwardings { get; set; }
        public bool AllowLiveComments { get; set; }
        public object ApprovedAtUtc { get; set; }
        public object ApprovedBy { get; set; }
        public bool Archived { get; set; }
        public string Author { get; set; }
        public object AuthorFlairBackgroundColor { get; set; }
        public string AuthorFlairCssClass { get; set; }
        public AuthorFlairRichtext[] AuthorFlairRichtext { get; set; }
        public Guid? AuthorFlairTemplateId { get; set; }
        public string AuthorFlairText { get; set; }
        public FlairTextColor? AuthorFlairTextColor { get; set; }
        public AuthorFlairType AuthorFlairType { get; set; }
        public string AuthorFullname { get; set; }
        public bool AuthorPatreonFlair { get; set; }
        public bool AuthorPremium { get; set; }
        public object[] Awarders { get; set; }
        public object BannedAtUtc { get; set; }
        public object BannedBy { get; set; }
        public bool CanGild { get; set; }
        public bool CanModPost { get; set; }
        public object Category { get; set; }
        public bool Clicked { get; set; }
        public object ContentCategories { get; set; }
        public bool ContestMode { get; set; }
        public long Created { get; set; }
        public long CreatedUtc { get; set; }
        public object DiscussionType { get; set; }
        public object Distinguished { get; set; }
        public string Domain { get; set; }
        public long Downs { get; set; }
        public bool Edited { get; set; }
        public long Gilded { get; set; }
        public Gildings Gildings { get; set; }
        public bool Hidden { get; set; }
        public bool HideScore { get; set; }
        public string Id { get; set; }
        public bool IsCrosspostable { get; set; }
        public bool IsMeta { get; set; }
        public bool IsOriginalContent { get; set; }
        public bool IsRedditMediaDomain { get; set; }
        public bool IsRobotIndexable { get; set; }
        public bool IsSelf { get; set; }
        public bool IsVideo { get; set; }
        public object Likes { get; set; }
        public string LinkFlairBackgroundColor { get; set; }
        public object LinkFlairCssClass { get; set; }
        public object[] LinkFlairRichtext { get; set; }
        public object LinkFlairText { get; set; }
        public FlairTextColor LinkFlairTextColor { get; set; }
        public AuthorFlairType LinkFlairType { get; set; }
        public bool Locked { get; set; }
        public CrosspostParentListMedia Media { get; set; }
        public MediaEmbed MediaEmbed { get; set; }
        public bool MediaOnly { get; set; }
        public object ModNote { get; set; }
        public object ModReasonBy { get; set; }
        public object ModReasonTitle { get; set; }
        public object[] ModReports { get; set; }
        public string Name { get; set; }
        public bool NoFollow { get; set; }
        public long NumComments { get; set; }
        public long NumCrossposts { get; set; }
        public object NumReports { get; set; }
        public bool Over18 { get; set; }
        public string ParentWhitelistStatus { get; set; }
        public string Permalink { get; set; }
        public bool Pinned { get; set; }
        public string PostHint { get; set; }
        public Preview Preview { get; set; }
        public long Pwls { get; set; }
        public bool Quarantine { get; set; }
        public object RemovalReason { get; set; }
        public object RemovedBy { get; set; }
        public object RemovedByCategory { get; set; }
        public object ReportReasons { get; set; }
        public bool Saved { get; set; }
        public long Score { get; set; }
        public CrosspostParentListMedia SecureMedia { get; set; }
        public MediaEmbed SecureMediaEmbed { get; set; }
        public string Selftext { get; set; }
        public object SelftextHtml { get; set; }
        public bool SendReplies { get; set; }
        public bool Spoiler { get; set; }
        public bool Stickied { get; set; }
        public string Subreddit { get; set; }
        public string SubredditId { get; set; }
        public string SubredditNamePrefixed { get; set; }
        public long SubredditSubscribers { get; set; }
        public SubredditType SubredditType { get; set; }
        public string SuggestedSort { get; set; }
        public Uri Thumbnail { get; set; }
        public long ThumbnailHeight { get; set; }
        public long ThumbnailWidth { get; set; }
        public string Title { get; set; }
        public object TopAwardedType { get; set; }
        public long TotalAwardsReceived { get; set; }
        public object[] TreatmentTags { get; set; }
        public long Ups { get; set; }
        public double UpvoteRatio { get; set; }
        public Uri Url { get; set; }
        public Uri UrlOverriddenByDest { get; set; }
        public object[] UserReports { get; set; }
        public object ViewCount { get; set; }
        public bool Visited { get; set; }
        public string WhitelistStatus { get; set; }
        public long Wls { get; set; }
    }

    public partial class Gildings
    {
    }

    public partial class CrosspostParentListMedia
    {
        public PurpleOembed Oembed { get; set; }
        public string Type { get; set; }
        public RedditVideo RedditVideo { get; set; }
    }

    public partial class PurpleOembed
    {
        public string AuthorName { get; set; }
        public Uri AuthorUrl { get; set; }
        public long CacheAge { get; set; }
        public long Height { get; set; }
        public string Html { get; set; }
        public string ProviderName { get; set; }
        public Uri ProviderUrl { get; set; }
        public string Type { get; set; }
        public Uri Url { get; set; }
        public string Version { get; set; }
        public long Width { get; set; }
    }

    public partial class RedditVideo
    {
        public Uri DashUrl { get; set; }
        public long Duration { get; set; }
        public Uri FallbackUrl { get; set; }
        public long Height { get; set; }
        public Uri HlsUrl { get; set; }
        public bool IsGif { get; set; }
        public Uri ScrubberMediaUrl { get; set; }
        public string TranscodingStatus { get; set; }
        public long Width { get; set; }
    }

    public partial class MediaEmbed
    {
        public string Content { get; set; }
        public long? Height { get; set; }
        public bool? Scrolling { get; set; }
        public long? Width { get; set; }
        public Uri MediaDomainUrl { get; set; }
    }

    public partial class Preview
    {
        public bool Enabled { get; set; }
        public Image[] Images { get; set; }
    }

    public partial class Image
    {
        public string Id { get; set; }
        public Source[] Resolutions { get; set; }
        public Source Source { get; set; }
        public Gildings Variants { get; set; }
    }

    public partial class Source
    {
        public long Height { get; set; }
        public Uri Url { get; set; }
        public long Width { get; set; }
    }

    public partial class RedditPostMedia
    {
        public FluffyOembed Oembed { get; set; }
        public string Type { get; set; }
    }

    public partial class FluffyOembed
    {
        public string AuthorName { get; set; }
        public Uri AuthorUrl { get; set; }
        public long? Height { get; set; }
        public string Html { get; set; }
        public string ProviderName { get; set; }
        public Uri ProviderUrl { get; set; }
        public long? ThumbnailHeight { get; set; }
        public Uri ThumbnailUrl { get; set; }
        public long? ThumbnailWidth { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Version { get; set; }
        public long Width { get; set; }
        public string Description { get; set; }
        public long? CacheAge { get; set; }
        public Uri Url { get; set; }
    }

    public enum AuthorFlairType { Richtext, Text };

    public enum FlairTextColor { Dark, Empty };

    public enum SubredditType { Public };

    public enum PostHint { Image, Link, RichVideo, Self };

    public enum PostEnum { Stupidpol };

    public enum SubredditId { T5Hitz3 };

    public enum ThumbnailEnum { Default, Self };

    public partial struct ThumbnailUnion
    {
        public ThumbnailEnum? Enum;
        public Uri PurpleUri;

        public static implicit operator ThumbnailUnion(ThumbnailEnum Enum) => new ThumbnailUnion { Enum = Enum };
        public static implicit operator ThumbnailUnion(Uri PurpleUri) => new ThumbnailUnion { PurpleUri = PurpleUri };
    }

}
