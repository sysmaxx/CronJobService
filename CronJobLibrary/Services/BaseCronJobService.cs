using CronJobLibrary.Models;
using Cronos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CronJobLibrary.Services
{
    public abstract class BaseCronJobService<T> : IBaseCronJobService<T> where T : BaseCronJobService<T>
    {

        private readonly CronExpression _expression;
        private readonly TimeZoneInfo _timeZoneInfo;
        private readonly bool _skipIfIsBusy;

        private readonly System.Timers.Timer _timer;

        private bool IsBusy { get; set; } = false;

        protected DateTimeOffset? _lastIteration;
        protected DateTimeOffset? NextIteration => _expression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);

        protected BaseCronJobService(IScheduleConfig<T> config)
        {
            _expression = CronExpression.Parse(config.CronJobExpression ?? throw new ArgumentNullException(nameof(config.CronJobExpression)));
            _timeZoneInfo = config.TimeZoneInfo ?? throw new ArgumentNullException(nameof(config.TimeZoneInfo));
            _skipIfIsBusy = config.SkipTaskIfBusy;

            _timer = new System.Timers.Timer();
            _timer.Elapsed += TimerElapsed;
        }

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            ResetTimer();
            return Task.CompletedTask;
        }

        private void ResetTimer()
        {
            _timer.Stop();
            var execution = DateTimeOffset.Now;
            var delay = (NextIteration.GetValueOrDefault(execution)) - execution;
            _timer.Interval = delay.TotalMilliseconds;
            _timer.Start();
        }

        private async void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ResetTimer();

            if (IsBusy & _skipIfIsBusy)
                return;

            IsBusy = true;
            await DoWork(CancellationToken.None);
            IsBusy = false;
        }

        public virtual async Task DoWork(CancellationToken cancellationToken)
        {
            // Placeholder... should be overwritten in the inheriting class
            await Task.Delay(1000, cancellationToken);  
        }


        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Stop();
            Dispose();
            await Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
