
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using reddit_scraper.data_holders;
using System;
using System.Globalization;

namespace reddit_scraper.DataHolders.PostResponseParser
{

    public partial class PostResponse
    {
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public Post[] Posts { get; set; }

        [JsonProperty("metadata", NullValueHandling = NullValueHandling.Ignore)]
        public Metadata Metadata { get; set; }
    }
#nullable enable
    public partial class Post
    {
        [JsonProperty("author", NullValueHandling = NullValueHandling.Ignore)]
        public string? Author { get; set; }

        [JsonProperty("author_flair_css_class")]
        public string? AuthorFlairCssClass { get; set; }

        [JsonProperty("author_flair_text")]
        public string? AuthorFlairText { get; set; }

        [JsonProperty("brand_safe", NullValueHandling = NullValueHandling.Ignore)]
        public bool? BrandSafe { get; set; }

        [JsonProperty("can_mod_post", NullValueHandling = NullValueHandling.Ignore)]
        public bool? CanModPost { get; set; }

        [JsonProperty("contest_mode", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ContestMode { get; set; }

        [JsonProperty("created_utc", NullValueHandling = NullValueHandling.Ignore)]
        public long CreatedUtc { get; set; }

        [JsonProperty("domain", NullValueHandling = NullValueHandling.Ignore)]
        public string? Domain { get; set; }

        [JsonProperty("full_link", NullValueHandling = NullValueHandling.Ignore)]
        public Uri? FullLink { get; set; }

        [JsonProperty("gilded", NullValueHandling = NullValueHandling.Ignore)]
        public long? Gilded { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string? Id { get; set; }

        [JsonProperty("is_crosspostable", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsCrosspostable { get; set; }

        [JsonProperty("is_reddit_media_domain", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsRedditMediaDomain { get; set; }

        [JsonProperty("is_self", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsSelf { get; set; }

        [JsonProperty("is_video", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsVideo { get; set; }

        [JsonProperty("locked", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Locked { get; set; }

        [JsonProperty("num_comments", NullValueHandling = NullValueHandling.Ignore)]
        public long? NumComments { get; set; }

        [JsonProperty("num_crossposts", NullValueHandling = NullValueHandling.Ignore)]
        public long? NumCrossposts { get; set; }

        [JsonProperty("over_18", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Over18 { get; set; }

        [JsonProperty("parent_whitelist_status", NullValueHandling = NullValueHandling.Ignore)]
        public string? ParentWhitelistStatus { get; set; }

        [JsonProperty("permalink", NullValueHandling = NullValueHandling.Ignore)]
        public string? Permalink { get; set; }

        [JsonProperty("pinned", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Pinned { get; set; }

        [JsonProperty("post_hint", NullValueHandling = NullValueHandling.Ignore)]
        public string? PostHint { get; set; }

        [JsonProperty("preview", NullValueHandling = NullValueHandling.Ignore)]
        public object? Preview { get; set; }

        [JsonProperty("retrieved_on", NullValueHandling = NullValueHandling.Ignore)]
        public long? RetrievedOn { get; set; }

        [JsonProperty("score", NullValueHandling = NullValueHandling.Ignore)]
        public long? Score { get; set; }

        [JsonProperty("selftext", NullValueHandling = NullValueHandling.Ignore)]
        public string? Selftext { get; set; }

        [JsonProperty("spoiler", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Spoiler { get; set; }

        [JsonProperty("stickied", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Stickied { get; set; }

        [JsonProperty("subreddit", NullValueHandling = NullValueHandling.Ignore)]
        public string? Subreddit { get; set; }

        [JsonProperty("subreddit_id", NullValueHandling = NullValueHandling.Ignore)]
        public string? SubredditId { get; set; }

        [JsonProperty("subreddit_type", NullValueHandling = NullValueHandling.Ignore)]
        public string? SubredditType { get; set; }

        [JsonProperty("thumbnail", NullValueHandling = NullValueHandling.Ignore)]
        public string? Thumbnail { get; set; }

        [JsonProperty("thumbnail_height", NullValueHandling = NullValueHandling.Ignore)]
        public long? ThumbnailHeight { get; set; }

        [JsonProperty("thumbnail_width", NullValueHandling = NullValueHandling.Ignore)]
        public long? ThumbnailWidth { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string? Title { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri? Url { get; set; }

        [JsonProperty("whitelist_status", NullValueHandling = NullValueHandling.Ignore)]
        public string? WhitelistStatus { get; set; }

        [JsonProperty("author_created_utc", NullValueHandling = NullValueHandling.Ignore)]
        public long? AuthorCreatedUtc { get; set; }

        [JsonProperty("author_fullname", NullValueHandling = NullValueHandling.Ignore)]
        public string? AuthorFullname { get; set; }

        [JsonProperty("media_embed", NullValueHandling = NullValueHandling.Ignore)]
        public object? MediaEmbed { get; set; }

        [JsonProperty("secure_media_embed", NullValueHandling = NullValueHandling.Ignore)]
        public object? SecureMediaEmbed { get; set; }

        [JsonProperty("media", NullValueHandling = NullValueHandling.Ignore)]
        public object? Media { get; set; }

        [JsonProperty("secure_media", NullValueHandling = NullValueHandling.Ignore)]
        public object? SecureMedia { get; set; }

        [JsonProperty("author_flair_richtext", NullValueHandling = NullValueHandling.Ignore)]
        public object?[]? AuthorFlairRichtext { get; set; }

        [JsonProperty("author_flair_type", NullValueHandling = NullValueHandling.Ignore)]
        public string? AuthorFlairType { get; set; }

        [JsonProperty("is_meta", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsMeta { get; set; }

        [JsonProperty("is_original_content", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsOriginalContent { get; set; }

        [JsonProperty("link_flair_background_color", NullValueHandling = NullValueHandling.Ignore)]
        public string? LinkFlairBackgroundColor { get; set; }

        [JsonProperty("link_flair_richtext", NullValueHandling = NullValueHandling.Ignore)]
        public object?[]? LinkFlairRichtext { get; set; }

        [JsonProperty("link_flair_text_color", NullValueHandling = NullValueHandling.Ignore)]
        public string? LinkFlairTextColor { get; set; }

        [JsonProperty("link_flair_type", NullValueHandling = NullValueHandling.Ignore)]
        public string? LinkFlairType { get; set; }

        [JsonProperty("media_only", NullValueHandling = NullValueHandling.Ignore)]
        public bool? MediaOnly { get; set; }

        [JsonProperty("no_follow", NullValueHandling = NullValueHandling.Ignore)]
        public bool? NoFollow { get; set; }

        [JsonProperty("pwls", NullValueHandling = NullValueHandling.Ignore)]
        public long? Pwls { get; set; }

        [JsonProperty("send_replies", NullValueHandling = NullValueHandling.Ignore)]
        public bool? SendReplies { get; set; }

        [JsonProperty("subreddit_subscribers", NullValueHandling = NullValueHandling.Ignore)]
        public long? SubredditSubscribers { get; set; }

        [JsonProperty("wls", NullValueHandling = NullValueHandling.Ignore)]
        public long? Wls { get; set; }

        [JsonProperty("approved_at_utc")]
        public object? ApprovedAtUtc { get; set; }

        [JsonProperty("banned_at_utc")]
        public object? BannedAtUtc { get; set; }

        [JsonProperty("suggested_sort")]
        public object? SuggestedSort { get; set; }

        [JsonProperty("rte_mode", NullValueHandling = NullValueHandling.Ignore)]
        public string? RteMode { get; set; }

        [JsonProperty("all_awardings", NullValueHandling = NullValueHandling.Ignore)]
        public object?[]? AllAwardings { get; set; }

        [JsonProperty("allow_live_comments", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AllowLiveComments { get; set; }

        [JsonProperty("author_patreon_flair", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AuthorPatreonFlair { get; set; }

        [JsonProperty("awarders", NullValueHandling = NullValueHandling.Ignore)]
        public object?[]? Awarders { get; set; }

        [JsonProperty("gildings", NullValueHandling = NullValueHandling.Ignore)]
        public object? Gildings { get; set; }

        [JsonProperty("is_robot_indexable", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsRobotIndexable { get; set; }

        [JsonProperty("steward_reports", NullValueHandling = NullValueHandling.Ignore)]
        public object?[]? StewardReports { get; set; }

        [JsonProperty("total_awards_received", NullValueHandling = NullValueHandling.Ignore)]
        public long? TotalAwardsReceived { get; set; }

        [JsonProperty("updated_utc", NullValueHandling = NullValueHandling.Ignore)]
        public long? UpdatedUtc { get; set; }

        [JsonProperty("author_flair_background_color", NullValueHandling = NullValueHandling.Ignore)]
        public string? AuthorFlairBackgroundColor { get; set; }

        [JsonProperty("author_flair_text_color", NullValueHandling = NullValueHandling.Ignore)]
        public string? AuthorFlairTextColor { get; set; }

        [JsonProperty("author_id", NullValueHandling = NullValueHandling.Ignore)]
        public string? AuthorId { get; set; }

        [JsonProperty("author_premium", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AuthorPremium { get; set; }

        [JsonProperty("treatment_tags", NullValueHandling = NullValueHandling.Ignore)]
        public object?[]? TreatmentTags { get; set; }

        [JsonProperty("view_count")]
        public object? ViewCount { get; set; }

        [JsonProperty("upvote_ratio", NullValueHandling = NullValueHandling.Ignore)]
        public double? UpvoteRatio { get; set; }

        [JsonProperty("url_overridden_by_dest", NullValueHandling = NullValueHandling.Ignore)]
        public Uri? UrlOverriddenByDest { get; set; }

        [JsonProperty("link_flair_text", NullValueHandling = NullValueHandling.Ignore)]
        public string? LinkFlairText { get; set; }

        [JsonProperty("link_flair_css_class", NullValueHandling = NullValueHandling.Ignore)]
        public string? LinkFlairCssClass { get; set; }

        [JsonProperty("og_description", NullValueHandling = NullValueHandling.Ignore)]
        public string? OgDescription { get; set; }

        [JsonProperty("og_title", NullValueHandling = NullValueHandling.Ignore)]
        public string? OgTitle { get; set; }
    }
#nullable disable
    public partial class PostResponse
    {
        public static PostResponse FromJson(string json) => JsonConvert.DeserializeObject<PostResponse>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this PostResponse self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
