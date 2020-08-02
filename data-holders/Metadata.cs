
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;
namespace reddit_scraper.data_holders
{
    public partial class Metadata
    {
        [JsonProperty("after", NullValueHandling = NullValueHandling.Ignore)]
        public long? After { get; set; }

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
        [JsonConverter(typeof(PurpleParseStringConverter))]
        public long? Limit { get; set; }

        [JsonProperty("metadata", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(FluffyParseStringConverter))]
        public bool? IsMetadata { get; set; }

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
        [JsonProperty("gt", NullValueHandling = NullValueHandling.Ignore)]
        public long? Gt { get; set; }

        [JsonProperty("lt", NullValueHandling = NullValueHandling.Ignore)]
        public long? Lt { get; set; }
    }

    public partial class Terms
    {
        [JsonProperty("subreddit", NullValueHandling = NullValueHandling.Ignore)]
        public string[] Subreddit { get; set; }
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

    public partial class Metadata
    {
        public static Metadata FromJson(string json) => JsonConvert.DeserializeObject<Metadata>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Metadata self) => JsonConvert.SerializeObject(self, Converter.Settings);
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

    internal class PurpleParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (long.TryParse(value, out var l)) {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null) {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly PurpleParseStringConverter Singleton = new PurpleParseStringConverter();
    }

    internal class FluffyParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(bool) || t == typeof(bool?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            bool b;
            if (bool.TryParse(value, out b)) {
                return b;
            }
            throw new Exception("Cannot unmarshal type bool");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null) {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (bool)untypedValue;
            var boolString = value ? "true" : "false";
            serializer.Serialize(writer, boolString);
            return;
        }

        public static readonly FluffyParseStringConverter Singleton = new FluffyParseStringConverter();
    }
}
