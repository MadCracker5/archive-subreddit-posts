using System;

namespace reddit_scraper
{
    public struct DateRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public static double TotalSecondsFromEpoch(DateTime date) =>
            date.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp) =>
             new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(unixTimeStamp)
                .ToLocalTime();
    }
}
