using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Common.BackgroundQueue
{
    /// <summary>
    /// Hosted service for background queue processing.
    /// </summary>
    public class QueuedHostedService : BackgroundService
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly ILogger _logger;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="taskQueue">Task queue.</param>
        /// <param name="loggerFactory">Logger factory.</param>
        public QueuedHostedService(IBackgroundTaskQueue taskQueue, ILoggerFactory loggerFactory)
        {
            _taskQueue = taskQueue;
            _logger = loggerFactory.CreateLogger<QueuedHostedService>();
        }

        /// <summary>
        /// Starts background queue processing.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task.</returns>
        protected async override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Queued Hosted Service is starting.");

            while (!cancellationToken.IsCancellationRequested)
            {
                var workItem = await _taskQueue.DequeueAsync(cancellationToken);

                try
                {
                    await workItem(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error occurred executing {nameof(workItem)}.");
                }
            }

            _logger.LogInformation("Queued Hosted Service is stopping.");
        }
    }

    /// <summary>
    /// Hosted service for background queue processing.
    /// </summary>
    public class QueuedHostedService<TService> : QueuedHostedService
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="taskQueue">Task queue.</param>
        /// <param name="loggerFactory">Logger factory.</param>
        public QueuedHostedService(IBackgroundTaskQueue<TService> taskQueue, ILoggerFactory loggerFactory)
            : base(taskQueue, loggerFactory)
        {
        }
    }
}




