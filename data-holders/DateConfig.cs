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
    public class Interval
    {
        private const int MillesecondsInDay = 86400 * 1000;
        private const int SecondsInDay = 86400;
        private const int HoursInDay = 24;
        public int DailyTimestep { get; set; }
        public Func<DateTime, int, DateTime> GetNextTimestep { get; set; }
        public Func<DateTime, int, long> OffsetBeforeFn { get; set; }
        public string Type { get; set; }
        public int Value { get; set; }
        private DateTime FnByMilleseconds(DateTime time, int y) =>
             time.AddMilliseconds(y * Value);
        private DateTime FnBySeconds(DateTime time, int y) =>
             time.AddSeconds(y * Value);
        private DateTime FnByHours(DateTime time, int y) =>
            time.AddHours(y * Value);
        private int IntervalByMilleseconds() =>
            (int)Math.Round((float)MillesecondsInDay / (float)Value);
        private int IntervalBySeconds() =>
            (int)Math.Round((float)SecondsInDay / (float)Value);
        private int IntervalByHours() =>
            (int)Math.Round((float)HoursInDay / (float)Value);
        private long OffsetBeforeFnByMilleseconds(DateTime time, int y) =>
            DateRange.TotalSecondsFromEpoch(time.AddMilliseconds(y == DailyTimestep ? Value - 1 : Value));
        private long OffsetBeforeFnBySeconds(DateTime time, int y) =>
            DateRange.TotalSecondsFromEpoch(time.AddSeconds(y == DailyTimestep ? Value - 1 : Value));
        private long OffsetBeforeFnByHours(DateTime time, int y) =>
            DateRange.TotalSecondsFromEpoch(time.AddHours(y == DailyTimestep ? Value - 1 : Value));
        private void TimeStepFnSwitch() =>
            GetNextTimestep = (Type.ToLowerInvariant()) switch
            {
                "seconds" => FnBySeconds,
                "second" => FnBySeconds,
                "s" => FnBySeconds,
                "hours" => FnByHours,
                "hour" => FnByHours,
                "h" => FnByHours,
                "milleseconds" => FnByMilleseconds,
                "millesecond" => FnByMilleseconds,
                "ms" => FnByMilleseconds,
                "m" => FnByMilleseconds,
                _ => FnBySeconds,
            };
        private void OffsetBeforeFnSwitch() =>
            OffsetBeforeFn = (Type.ToLowerInvariant()) switch
            {
                "seconds" => OffsetBeforeFnBySeconds,
                "second" => OffsetBeforeFnBySeconds,
                "s" => OffsetBeforeFnBySeconds,
                "hours" => OffsetBeforeFnByHours,
                "hour" => OffsetBeforeFnByHours,
                "h" => OffsetBeforeFnByHours,
                "milleseconds" => OffsetBeforeFnByMilleseconds,
                "millesecond" => OffsetBeforeFnByMilleseconds,
                "ms" => OffsetBeforeFnByMilleseconds,
                "m" => OffsetBeforeFnByMilleseconds,
                _ => OffsetBeforeFnBySeconds,
            };
        private void IntervalTypeSwitch() =>
            DailyTimestep = ((Type.ToLowerInvariant()) switch
            {
                "seconds" => IntervalBySeconds(),
                "second" => IntervalBySeconds(),
                "s" => IntervalBySeconds(),
                "hours" => IntervalByHours(),
                "hour" => IntervalByHours(),
                "h" => IntervalByHours(),
                "milleseconds" => IntervalByMilleseconds(),
                "millesecond" => IntervalByMilleseconds(),
                "ms" => IntervalByMilleseconds(),
                "m" => IntervalByMilleseconds(),
                _ => IntervalBySeconds(),
            });
        public Interval ResolveInterval()
        {
            TimeStepFnSwitch();
            IntervalTypeSwitch();
            OffsetBeforeFnSwitch();
            return this;
        }
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
        public static Interval ParseInterval(IConfigurationRoot configuration)
        {
            var tt = configuration.GetSection("interval").Get<Interval>().ResolveInterval();
            Console.WriteLine(tt);
            return tt;
        }
        public static DateTime ParseDateCutoffSection(IConfigurationRoot configuration, DateConfigEnum dateType) =>
            configuration.GetSection(dateType == DateConfigEnum.After ? "after" : "before").Get<DateTimeShuttle>().ToDateTime();
    }
}
