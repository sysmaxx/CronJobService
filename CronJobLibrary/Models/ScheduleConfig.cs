using System;

namespace CronJobLibrary.Models
{
    public class ScheduleConfig<T> : IScheduleConfig<T>
    {
        public string CronJobExpression { get; set; }
        public TimeZoneInfo TimeZoneInfo { get; set; }
    }
}
