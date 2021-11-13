using System;
namespace CronJobLibrary.Models
{
    public interface IScheduleConfig<T>
    {
        string CronJobExpression { get; set; }
        TimeZoneInfo TimeZoneInfo { get; set; }
        bool SkipTaskIfBusy { get; set; }
    }
}
