using System;
using System.Collections;
using System.Collections.Generic;

namespace reddit_scraper.Tools
{
    public struct DateRange
    {
        //public DateTime After { get; set; }
        //public DateTime Before { get; set; }
        public long After { get; set; }
        public long Before { get; set; }
        public static long TotalSecondsFromEpoch(DateTime date) =>
            (long)date.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp) =>
             new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(unixTimeStamp);
    }
    //public struct DayDateRanges
    //{
    //    public List<DayDateRange> DateRanges { get; set; }
    //}
    public struct DayDateRange
    {
        public DateTime Date { get; set; }
        public List<DateRange> Intervals { get; set; }
    }
}
