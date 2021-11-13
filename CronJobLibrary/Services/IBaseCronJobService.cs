using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CronJobLibrary.Services
{
    public interface IBaseCronJobService<T> : IHostedService, IDisposable
    {
        Task DoWork(CancellationToken cancellationToken);
    }
}
