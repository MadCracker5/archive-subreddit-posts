
using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace reddit_scraper.DataHolders.CommentResponseParser
{

    public partial class CommentResponse
    {
        [JsonProperty("data")]
        public Comment[] Comments { get; set; }
    }
#nullable enable
    public partial class Comment
    {
        [JsonProperty("all_awardings", NullValueHandling = NullValueHandling.Ignore)]
        public object?[]? AllAwardings { get; set; }

        [JsonProperty("approved_at_utc")]
        public long? ApprovedAtUtc { get; set; }

        [JsonProperty("associated_award")]
        public object? AssociatedAward { get; set; }

        [JsonProperty("author", NullValueHandling = NullValueHandling.Ignore)]
        public string? Author { get; set; }

        [JsonProperty("author_flair_background_color")]
        public object? AuthorFlairBackgroundColor { get; set; }

        [JsonProperty("author_flair_css_class")]
        public string? AuthorFlairCssClass { get; set; }

        [JsonProperty("author_flair_richtext", NullValueHandling = NullValueHandling.Ignore)]
        public object[]? AuthorFlairRichtext { get; set; }

        [JsonProperty("author_flair_template_id")]
        public string? AuthorFlairTemplateId { get; set; }

        [JsonProperty("author_flair_text")]
        public string? AuthorFlairText { get; set; }

        [JsonProperty("author_flair_text_color")]
        public string? AuthorFlairTextColor { get; set; }

        [JsonProperty("author_flair_type", NullValueHandling = NullValueHandling.Ignore)]
        public string? AuthorFlairType { get; set; }

        [JsonProperty("author_fullname", NullValueHandling = NullValueHandling.Ignore)]
        public string? AuthorFullname { get; set; }

        [JsonProperty("author_patreon_flair", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AuthorPatreonFlair { get; set; }

        [JsonProperty("author_premium", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AuthorPremium { get; set; }

        [JsonProperty("awarders", NullValueHandling = NullValueHandling.Ignore)]
        public object?[]? Awarders { get; set; }

        [JsonProperty("banned_at_utc")]
        public object? BannedAtUtc { get; set; }

        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        public string? Body { get; set; }

        [JsonProperty("can_mod_post", NullValueHandling = NullValueHandling.Ignore)]
        public bool? CanModPost { get; set; }

        [JsonProperty("collapsed", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Collapsed { get; set; }

        [JsonProperty("collapsed_because_crowd_control")]
        public object? CollapsedBecauseCrowdControl { get; set; }

        [JsonProperty("collapsed_reason")]
        public string? CollapsedReason { get; set; }

        [JsonProperty("created_utc", NullValueHandling = NullValueHandling.Ignore)]
        public long? CreatedUtc { get; set; }

        [JsonProperty("distinguished")]
        public object? Distinguished { get; set; }

        [JsonProperty("edited", NullValueHandling = NullValueHandling.Ignore)]
        public string? Edited { get; set; }

        [JsonProperty("gildings", NullValueHandling = NullValueHandling.Ignore)]
        public object? Gildings { get; set; }

        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string? Id { get; set; }

        [JsonProperty("is_submitter", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsSubmitter { get; set; }

        [JsonProperty("link_id", NullValueHandling = NullValueHandling.Ignore)]
        public string? LinkId { get; set; }

        [JsonProperty("locked", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Locked { get; set; }

        [JsonProperty("no_follow", NullValueHandling = NullValueHandling.Ignore)]
        public bool? NoFollow { get; set; }

        [JsonProperty("parent_id", NullValueHandling = NullValueHandling.Ignore)]
        public string? ParentId { get; set; }

        [JsonProperty("permalink", NullValueHandling = NullValueHandling.Ignore)]
        public string? Permalink { get; set; }

        [JsonProperty("retrieved_on", NullValueHandling = NullValueHandling.Ignore)]
        public long? RetrievedOn { get; set; }

        [JsonProperty("score", NullValueHandling = NullValueHandling.Ignore)]
        public long? Score { get; set; }

        [JsonProperty("send_replies", NullValueHandling = NullValueHandling.Ignore)]
        public bool? SendReplies { get; set; }

        [JsonProperty("stickied", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Stickied { get; set; }

        [JsonProperty("subreddit", NullValueHandling = NullValueHandling.Ignore)]
        public string? Subreddit { get; set; }

        [JsonProperty("subreddit_id", NullValueHandling = NullValueHandling.Ignore)]
        public string? SubredditId { get; set; }

        [JsonProperty("top_awarded_type")]
        public object? TopAwardedType { get; set; }

        [JsonProperty("total_awards_received", NullValueHandling = NullValueHandling.Ignore)]
        public long? TotalAwardsReceived { get; set; }

        [JsonProperty("treatment_tags", NullValueHandling = NullValueHandling.Ignore)]
        public object?[]? TreatmentTags { get; set; }
    }

#nullable disable
    public partial class CommentResponse
    {
        public static CommentResponse FromJson(string json) => JsonConvert.DeserializeObject<CommentResponse>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this CommentResponse self) => JsonConvert.SerializeObject(self, Converter.Settings);
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
