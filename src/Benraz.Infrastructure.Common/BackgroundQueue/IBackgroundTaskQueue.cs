using System;
using System.Threading;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Common.BackgroundQueue
{
    /// <summary>
    /// Background task queue.
    /// </summary>
    public interface IBackgroundTaskQueue
    {
        /// <summary>
        /// Enqueues new work item.
        /// </summary>
        /// <param name="workItem">Work item.</param>
        void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem);

        /// <summary>
        /// Dequeues work item.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Work item.</returns>
        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }

    /// <summary>
    /// Background task queue.
    /// </summary>
    public interface IBackgroundTaskQueue<TService> : IBackgroundTaskQueue
    {
    }
}




