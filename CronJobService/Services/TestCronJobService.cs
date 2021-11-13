using CronJobLibrary.Models;
using CronJobLibrary.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CronJobApi.Services
{
    public class TestCronJobService : BaseCronJobService
    {

        private readonly ILogger<TestCronJobService> _logger;

        public TestCronJobService(
            IScheduleConfig<TestCronJobService> config, 
            ILogger<TestCronJobService> logger) 
            : base(config.CronJobExpression, config.TimeZoneInfo)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} {nameof(TestCronJobService)} is starting. Next iterration is: {NextIteration.Value:dd.MM.yy hh:mm:ss}");
            return base.StartAsync(cancellationToken);
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} {nameof(TestCronJobService)} is working.");
            await Task.Delay(5000).ConfigureAwait(false);

            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} {nameof(TestCronJobService)} is done. Next iterration is: {NextIteration.Value:dd.MM.yy hh:mm:ss}");
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(TestCronJobService)} is stopping.");
            return base.StopAsync(cancellationToken);
        }

    }
}
