using CronJobLibrary.Models;
using CronJobLibrary.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CronJobLibrary.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCronJob<T>(this IServiceCollection services, Action<IScheduleConfig<T>> options) where T : BaseCronJobService<T>
        {
            if (options is null)
                throw new ArgumentNullException(nameof(options), "Schedule Configurations is missing");

            var config = new ScheduleConfig<T>();
            options.Invoke(config);
            if (string.IsNullOrWhiteSpace(config.CronJobExpression))
                throw new ArgumentNullException(nameof(ScheduleConfig<T>.CronJobExpression), "Empty CronJobExpression is not allowed!");

            services.AddSingleton<IScheduleConfig<T>>(config);
            services.AddHostedService<T>();
            return services;
        }
    }
}
