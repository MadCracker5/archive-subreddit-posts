using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

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
                return dateType == DateConfigEnum.After ? new DateTime(2007, 1, 1) : DateTime.Now;
            }
            var year = configuration.GetSection("year").Value;
            var month = configuration.GetSection("month").Value;
            var day = configuration.GetSection("day").Value;
            return new DateTime(
                year != null
                    ? int.Parse(year)
                    : dateType == DateConfigEnum.After
                        ? 2007
                        : DateTime.Now.Year,
                month != null
                    ? int.Parse(month)
                    : dateType == DateConfigEnum.After
                        ? 01
                        : DateTime.Now.Month,
               day != null
                    ? int.Parse(day)
                    : dateType == DateConfigEnum.After
                        ? 01
                        : DateTime.Now.Day
            );
        }
    }
}
