using System;

namespace reddit_scraper.Tools
{
    public struct DateRange
    {
        public DateTime After { get; set; }
        public DateTime Before { get; set; }
        public static long TotalSecondsFromEpoch(DateTime date) =>
            (long)date.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp) =>
             new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(unixTimeStamp);
    }
}
