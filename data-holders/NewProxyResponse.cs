using Newtonsoft.Json;
using System;

namespace reddit_scraper.DataHolders
{
    public partial class NewProxyResponse
    {
        [JsonProperty("supportsHttps")]
        public bool SupportsHttps { get; set; }

        [JsonProperty("protocol")]
        public string Protocol { get; set; }

        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("port")]
        public long Port { get; set; }

        [JsonProperty("get")]
        public bool Get { get; set; }

        [JsonProperty("post")]
        public bool Post { get; set; }

        [JsonProperty("cookies")]
        public bool Cookies { get; set; }

        [JsonProperty("referer")]
        public bool Referer { get; set; }

        [JsonProperty("user-agent")]
        public bool UserAgent { get; set; }

        [JsonProperty("anonymityLevel")]
        public long AnonymityLevel { get; set; }

        [JsonProperty("websites")]
        public Websites Websites { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("unixTimestampMs")]
        public long UnixTimestampMs { get; set; }

        [JsonProperty("tsChecked")]
        public long TsChecked { get; set; }

        [JsonProperty("unixTimestamp")]
        public long UnixTimestamp { get; set; }

        [JsonProperty("curl")]
        public Uri Curl { get; set; }

        [JsonProperty("ipPort")]
        public string IpPort { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("speed")]
        public double Speed { get; set; }

        [JsonProperty("otherProtocols")]
        public OtherProtocols OtherProtocols { get; set; }

        [JsonProperty("verifiedSecondsAgo")]
        public long VerifiedSecondsAgo { get; set; }
    }

    public partial class OtherProtocols
    {
    }

    public partial class Websites
    {
        [JsonProperty("example")]
        public bool Example { get; set; }

        [JsonProperty("google")]
        public bool Google { get; set; }

        [JsonProperty("amazon")]
        public bool Amazon { get; set; }

        [JsonProperty("yelp")]
        public bool Yelp { get; set; }

        [JsonProperty("google_maps")]
        public bool GoogleMaps { get; set; }
    }
}
