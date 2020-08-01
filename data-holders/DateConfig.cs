using Microsoft.Extensions.Configuration;
using System;

namespace reddit_scraper.DataHolders
{
    public enum DateConfigEnum
    {
        Before,
        After
    }
    public class DateConfig
    {
#nullable enable
        public static DateTime ParseSection(IConfigurationSection? configuration, DateConfigEnum dateType)
        {
            if (configuration == null) {
                return dateType == DateConfigEnum.After ? new DateTime(2007, 1, 1) : DateTime.UtcNow;
            }
            var year = configuration.GetSection("year").Value;
            var month = configuration.GetSection("month").Value;
            var day = configuration.GetSection("day").Value;
            return new DateTime(
                year != null
                    ? int.Parse(year)
                    : dateType == DateConfigEnum.After
                        ? 2007
                        : DateTime.UtcNow.Year,
                month != null
                    ? int.Parse(month)
                    : dateType == DateConfigEnum.After
                        ? 01
                        : DateTime.UtcNow.Month,
               day != null
                    ? int.Parse(day)
                    : dateType == DateConfigEnum.After
                        ? 01
                        : DateTime.UtcNow.Day, 
               0,
               0,
               0,
               DateTimeKind.Utc
            );
        }
    }
}
