using Microsoft.Extensions.Configuration;
using reddit_scraper.Tools;
using System;

namespace reddit_scraper.DataHolders
{
    public enum DateConfigEnum
    {
        Before,
        After
    }
    public class DateTimeShuttle
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public DateTime ToDateTime() =>
             new DateTime(Year, Month, Day, 0, 0, 0, DateTimeKind.Utc);
    }
    public class DateConfig
    {
#nullable enable
        public static DateTime ParseDateCutoffSection(IConfigurationRoot configuration, DateConfigEnum dateType) =>
            configuration.GetSection(dateType == DateConfigEnum.After ? "after" : "before").Get<DateTimeShuttle>().ToDateTime();
    }
}
