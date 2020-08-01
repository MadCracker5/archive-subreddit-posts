using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace reddit_scraper.DataHolders
{
    public partial class PushshiftMetaInfo
    {
        [JsonProperty("client_accepts_json")]
        public bool ClientAcceptsJson { get; set; }

        [JsonProperty("client_request_headers")]
        public ClientRequestHeaders ClientRequestHeaders { get; set; }

        [JsonProperty("client_user_agent")]
        public string ClientUserAgent { get; set; }

        [JsonProperty("server_ratelimit_per_minute")]
        public int ServerRatelimitPerMinute { get; set; }

        [JsonProperty("source-ip")]
        public string SourceIp { get; set; }
    }

    public partial class ClientRequestHeaders
    {
        [JsonProperty("ACCEPT")]
        public string Accept { get; set; }

        [JsonProperty("ACCEPT-ENCODING")]
        public string AcceptEncoding { get; set; }

        [JsonProperty("CACHE-CONTROL")]
        public string CacheControl { get; set; }

        [JsonProperty("CDN-LOOP")]
        public string CdnLoop { get; set; }

        [JsonProperty("CF-CONNECTING-IP")]
        public string CfConnectingIp { get; set; }

        [JsonProperty("CF-IPCOUNTRY")]
        public string CfIpcountry { get; set; }

        [JsonProperty("CF-RAY")]
        public string CfRay { get; set; }

        [JsonProperty("CF-REQUEST-ID")]
        public string CfRequestId { get; set; }

        [JsonProperty("CF-VISITOR")]
        public string CfVisitor { get; set; }

        [JsonProperty("CONNECTION")]
        public string Connection { get; set; }

        [JsonProperty("CONTENT-LENGTH")]
        public int ContentLength { get; set; }

        [JsonProperty("CONTENT-TYPE")]
        public string ContentType { get; set; }

        [JsonProperty("HOST")]
        public string Host { get; set; }

        [JsonProperty("POSTMAN-TOKEN")]
        public Guid PostmanToken { get; set; }

        [JsonProperty("USER-AGENT")]
        public string UserAgent { get; set; }

        [JsonProperty("X-FORWARDED-FOR")]
        public string XForwardedFor { get; set; }

        [JsonProperty("X-FORWARDED-PROTO")]
        public string XForwardedProto { get; set; }
    }
}
