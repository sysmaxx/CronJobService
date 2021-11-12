using Cronos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CronJobLibrary.Services
{
    public abstract class BaseCronJobService : IBaseCronJobService
    {

        private readonly CronExpression _expression;
        private readonly TimeZoneInfo _timeZoneInfo;

        private readonly System.Timers.Timer _timer;

        protected DateTimeOffset? _lastIteration;
        protected DateTimeOffset? NextIteration => _expression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);

        protected BaseCronJobService(string expression, TimeZoneInfo timeZoneInfo)
        {
            _expression = CronExpression.Parse(expression ?? throw new ArgumentNullException(nameof(expression)));
            _timeZoneInfo = timeZoneInfo ?? throw new ArgumentNullException(nameof(timeZoneInfo));

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
            await DoWork(CancellationToken.None);
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
