
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using reddit_scraper.data_holders;
using System;
using System.Globalization;

namespace reddit_scraper
{

    public partial class PostResponse
    {
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public Post[] Posts { get; set; }

        [JsonProperty("metadata", NullValueHandling = NullValueHandling.Ignore)]
        public Metadata Metadata { get; set; }
    }

    public partial class Post
    {
        [JsonProperty("all_awardings", NullValueHandling = NullValueHandling.Ignore)]
        public object[] AllAwardings { get; set; }

        [JsonProperty("allow_live_comments", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AllowLiveComments { get; set; }

        [JsonProperty("author", NullValueHandling = NullValueHandling.Ignore)]
        public string Author { get; set; }

        [JsonProperty("author_flair_css_class")]
        public string AuthorFlairCssClass { get; set; }

        [JsonProperty("author_flair_richtext", NullValueHandling = NullValueHandling.Ignore)]
        public object[] AuthorFlairRichtext { get; set; }

        [JsonProperty("author_flair_text")]
        public string AuthorFlairText { get; set; }

        [JsonProperty("author_flair_type", NullValueHandling = NullValueHandling.Ignore)]
        public object? AuthorFlairType { get; set; }

        [JsonProperty("author_fullname", NullValueHandling = NullValueHandling.Ignore)]
        public string AuthorFullname { get; set; }

        [JsonProperty("author_patreon_flair", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AuthorPatreonFlair { get; set; }

        [JsonProperty("author_premium", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AuthorPremium { get; set; }

        [JsonProperty("awarders", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Awarders { get; set; }

        [JsonProperty("can_mod_post", NullValueHandling = NullValueHandling.Ignore)]
        public bool? CanModPost { get; set; }

        [JsonProperty("contest_mode", NullValueHandling = NullValueHandling.Ignore)]
        public bool? ContestMode { get; set; }

        [JsonProperty("created_utc", NullValueHandling = NullValueHandling.Ignore)]
        public long CreatedUtc { get; set; }

        [JsonProperty("domain", NullValueHandling = NullValueHandling.Ignore)]
        public string Domain { get; set; }

        [JsonProperty("full_link", NullValueHandling = NullValueHandling.Ignore)]
        public Uri FullLink { get; set; }

        [JsonProperty("gildings", NullValueHandling = NullValueHandling.Ignore)]
        public object Gildings { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("is_crosspostable", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsCrosspostable { get; set; }

        [JsonProperty("is_meta", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsMeta { get; set; }

        [JsonProperty("is_original_content", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsOriginalContent { get; set; }

        [JsonProperty("is_reddit_media_domain", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsRedditMediaDomain { get; set; }

        [JsonProperty("is_robot_indexable", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsRobotIndexable { get; set; }

        [JsonProperty("is_self", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsSelf { get; set; }

        [JsonProperty("is_video", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsVideo { get; set; }

        [JsonProperty("link_flair_background_color", NullValueHandling = NullValueHandling.Ignore)]
        public string LinkFlairBackgroundColor { get; set; }

        [JsonProperty("link_flair_css_class", NullValueHandling = NullValueHandling.Ignore)]
        public object? LinkFlairCssClass { get; set; }

        [JsonProperty("link_flair_richtext", NullValueHandling = NullValueHandling.Ignore)]
        public object[] LinkFlairRichtext { get; set; }

        [JsonProperty("link_flair_text", NullValueHandling = NullValueHandling.Ignore)]
        public object? LinkFlairText { get; set; }

        [JsonProperty("link_flair_text_color", NullValueHandling = NullValueHandling.Ignore)]
        public object? LinkFlairTextColor { get; set; }

        [JsonProperty("link_flair_type", NullValueHandling = NullValueHandling.Ignore)]
        public object? LinkFlairType { get; set; }

        [JsonProperty("locked", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Locked { get; set; }

        [JsonProperty("media_only", NullValueHandling = NullValueHandling.Ignore)]
        public bool? MediaOnly { get; set; }

        [JsonProperty("no_follow", NullValueHandling = NullValueHandling.Ignore)]
        public bool? NoFollow { get; set; }

        [JsonProperty("num_comments", NullValueHandling = NullValueHandling.Ignore)]
        public long? NumComments { get; set; }

        [JsonProperty("num_crossposts", NullValueHandling = NullValueHandling.Ignore)]
        public long? NumCrossposts { get; set; }

        [JsonProperty("over_18", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Over18 { get; set; }

        [JsonProperty("parent_whitelist_status", NullValueHandling = NullValueHandling.Ignore)]
        public object? ParentWhitelistStatus { get; set; }

        [JsonProperty("permalink", NullValueHandling = NullValueHandling.Ignore)]
        public string Permalink { get; set; }

        [JsonProperty("pinned", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Pinned { get; set; }

        [JsonProperty("post_hint", NullValueHandling = NullValueHandling.Ignore)]
        public object? PostHint { get; set; }

        [JsonProperty("preview", NullValueHandling = NullValueHandling.Ignore)]
        public object Preview { get; set; }

        [JsonProperty("pwls", NullValueHandling = NullValueHandling.Ignore)]
        public long? Pwls { get; set; }

        [JsonProperty("retrieved_on", NullValueHandling = NullValueHandling.Ignore)]
        public long? RetrievedOn { get; set; }

        [JsonProperty("score", NullValueHandling = NullValueHandling.Ignore)]
        public long? Score { get; set; }

        [JsonProperty("selftext", NullValueHandling = NullValueHandling.Ignore)]
        public string Selftext { get; set; }

        [JsonProperty("send_replies", NullValueHandling = NullValueHandling.Ignore)]
        public bool? SendReplies { get; set; }

        [JsonProperty("spoiler", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Spoiler { get; set; }

        [JsonProperty("stickied", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Stickied { get; set; }

        [JsonProperty("subreddit", NullValueHandling = NullValueHandling.Ignore)]
        public object? Subreddit { get; set; }

        [JsonProperty("subreddit_id", NullValueHandling = NullValueHandling.Ignore)]
        public object? SubredditId { get; set; }

        [JsonProperty("subreddit_subscribers", NullValueHandling = NullValueHandling.Ignore)]
        public long? SubredditSubscribers { get; set; }

        [JsonProperty("subreddit_type", NullValueHandling = NullValueHandling.Ignore)]
        public object? SubredditType { get; set; }

        [JsonProperty("suggested_sort", NullValueHandling = NullValueHandling.Ignore)]
        public object? SuggestedSort { get; set; }

        [JsonProperty("thumbnail", NullValueHandling = NullValueHandling.Ignore)]
        public object? Thumbnail { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("total_awards_received", NullValueHandling = NullValueHandling.Ignore)]
        public long? TotalAwardsReceived { get; set; }

        [JsonProperty("treatment_tags", NullValueHandling = NullValueHandling.Ignore)]
        public object[] TreatmentTags { get; set; }

        [JsonProperty("upvote_ratio", NullValueHandling = NullValueHandling.Ignore)]
        public double? UpvoteRatio { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Url { get; set; }

        [JsonProperty("whitelist_status", NullValueHandling = NullValueHandling.Ignore)]
        public object? WhitelistStatus { get; set; }

        [JsonProperty("wls", NullValueHandling = NullValueHandling.Ignore)]
        public long? Wls { get; set; }

        [JsonProperty("author_flair_background_color", NullValueHandling = NullValueHandling.Ignore)]
        public object? AuthorFlairBackgroundColor { get; set; }

        [JsonProperty("author_flair_text_color", NullValueHandling = NullValueHandling.Ignore)]
        public object? AuthorFlairTextColor { get; set; }

        [JsonProperty("removed_by_category", NullValueHandling = NullValueHandling.Ignore)]
        public object? RemovedByCategory { get; set; }

        [JsonProperty("author_flair_template_id", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? AuthorFlairTemplateId { get; set; }

        [JsonProperty("media", NullValueHandling = NullValueHandling.Ignore)]
        public object? Media { get; set; }

        [JsonProperty("media_embed", NullValueHandling = NullValueHandling.Ignore)]
        public object? MediaEmbed { get; set; }

        [JsonProperty("secure_media", NullValueHandling = NullValueHandling.Ignore)]
        public object? SecureMedia { get; set; }

        [JsonProperty("secure_media_embed", NullValueHandling = NullValueHandling.Ignore)]
        public object? SecureMediaEmbed { get; set; }

        [JsonProperty("thumbnail_height", NullValueHandling = NullValueHandling.Ignore)]
        public long? ThumbnailHeight { get; set; }

        [JsonProperty("thumbnail_width", NullValueHandling = NullValueHandling.Ignore)]
        public long? ThumbnailWidth { get; set; }

        [JsonProperty("url_overridden_by_dest", NullValueHandling = NullValueHandling.Ignore)]
        public Uri UrlOverriddenByDest { get; set; }

        [JsonProperty("link_flair_template_id", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? LinkFlairTemplateId { get; set; }

        [JsonProperty("media_metadata", NullValueHandling = NullValueHandling.Ignore)]
        public object MediaMetadata { get; set; }

        [JsonProperty("crosspost_parent", NullValueHandling = NullValueHandling.Ignore)]
        public string CrosspostParent { get; set; }

        [JsonProperty("crosspost_parent_list", NullValueHandling = NullValueHandling.Ignore)]
        public object[] CrosspostParentList { get; set; }
    }

    public partial class Metadata
    {
        [JsonProperty("after")]
        public object After { get; set; }

        [JsonProperty("agg_size", NullValueHandling = NullValueHandling.Ignore)]
        public long? AggSize { get; set; }

        [JsonProperty("api_version", NullValueHandling = NullValueHandling.Ignore)]
        public string ApiVersion { get; set; }

        [JsonProperty("before", NullValueHandling = NullValueHandling.Ignore)]
        public long? Before { get; set; }

        [JsonProperty("es_query", NullValueHandling = NullValueHandling.Ignore)]
        public EsQuery EsQuery { get; set; }

        [JsonProperty("execution_time_milliseconds", NullValueHandling = NullValueHandling.Ignore)]
        public double? ExecutionTimeMilliseconds { get; set; }

        [JsonProperty("index", NullValueHandling = NullValueHandling.Ignore)]
        public string Index { get; set; }

        [JsonProperty("limit", NullValueHandling = NullValueHandling.Ignore)]
        public long? Limit { get; set; }

        [JsonProperty("metadata", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsMedata { get; set; }

        [JsonProperty("ranges", NullValueHandling = NullValueHandling.Ignore)]
        public RangeElement[] Ranges { get; set; }

        [JsonProperty("results_returned", NullValueHandling = NullValueHandling.Ignore)]
        public long? ResultsReturned { get; set; }

        [JsonProperty("shards", NullValueHandling = NullValueHandling.Ignore)]
        public Shards Shards { get; set; }

        [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
        public long? Size { get; set; }

        [JsonProperty("sort", NullValueHandling = NullValueHandling.Ignore)]
        public string Sort { get; set; }

        [JsonProperty("sort_type", NullValueHandling = NullValueHandling.Ignore)]
        public string SortType { get; set; }

        [JsonProperty("subreddit", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Subreddit { get; set; }

        [JsonProperty("timed_out", NullValueHandling = NullValueHandling.Ignore)]
        public bool? TimedOut { get; set; }

        [JsonProperty("total_results", NullValueHandling = NullValueHandling.Ignore)]
        public long? TotalResults { get; set; }
    }

    public partial class EsQuery
    {
        [JsonProperty("query", NullValueHandling = NullValueHandling.Ignore)]
        public Query Query { get; set; }

        [JsonProperty("size", NullValueHandling = NullValueHandling.Ignore)]
        public long? Size { get; set; }

        [JsonProperty("sort", NullValueHandling = NullValueHandling.Ignore)]
        public Sort Sort { get; set; }
    }

    public partial class Query
    {
        [JsonProperty("bool", NullValueHandling = NullValueHandling.Ignore)]
        public QueryBool Bool { get; set; }
    }

    public partial class QueryBool
    {
        [JsonProperty("filter", NullValueHandling = NullValueHandling.Ignore)]
        public Filter Filter { get; set; }

        [JsonProperty("must_not", NullValueHandling = NullValueHandling.Ignore)]
        public object[] MustNot { get; set; }
    }

    public partial class Filter
    {
        [JsonProperty("bool", NullValueHandling = NullValueHandling.Ignore)]
        public FilterBool Bool { get; set; }
    }

    public partial class FilterBool
    {
        [JsonProperty("must", NullValueHandling = NullValueHandling.Ignore)]
        public Must[] Must { get; set; }

        [JsonProperty("should", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Should { get; set; }
    }

    public partial class Must
    {
        [JsonProperty("terms", NullValueHandling = NullValueHandling.Ignore)]
        public Terms Terms { get; set; }

        [JsonProperty("range", NullValueHandling = NullValueHandling.Ignore)]
        public MustRange Range { get; set; }
    }

    public partial class MustRange
    {
        [JsonProperty("created_utc", NullValueHandling = NullValueHandling.Ignore)]
        public CreatedUtc CreatedUtc { get; set; }
    }

    public partial class CreatedUtc
    {
        [JsonProperty("lt", NullValueHandling = NullValueHandling.Ignore)]
        public long? Lt { get; set; }
    }

    public partial class Terms
    {
        [JsonProperty("subreddit", NullValueHandling = NullValueHandling.Ignore)]
        public object[] Subreddit { get; set; }
    }

    public partial class Sort
    {
        [JsonProperty("created_utc", NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedUtc { get; set; }
    }

    public partial class RangeElement
    {
        [JsonProperty("range", NullValueHandling = NullValueHandling.Ignore)]
        public MustRange Range { get; set; }
    }

    public partial class Shards
    {
        [JsonProperty("failed", NullValueHandling = NullValueHandling.Ignore)]
        public long? Failed { get; set; }

        [JsonProperty("skipped", NullValueHandling = NullValueHandling.Ignore)]
        public long? Skipped { get; set; }

        [JsonProperty("successful", NullValueHandling = NullValueHandling.Ignore)]
        public long? Successful { get; set; }

        [JsonProperty("total", NullValueHandling = NullValueHandling.Ignore)]
        public long? Total { get; set; }
    }


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
