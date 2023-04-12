using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Common.BackgroundQueue
{
    /// <summary>
    /// Background task queue.
    /// </summary>
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly ConcurrentQueue<Func<CancellationToken, Task>> _workItems;
        private readonly SemaphoreSlim _signal;

        /// <summary>
        /// Constructor.
        /// </summary>
        public BackgroundTaskQueue()
        {
            _workItems = new ConcurrentQueue<Func<CancellationToken, Task>>();
            _signal = new SemaphoreSlim(0);
        }

        /// <summary>
        /// Enqueues new work item.
        /// </summary>
        /// <param name="workItem">Work item.</param>
        public void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            _workItems.Enqueue(workItem);
            _signal.Release();
        }

        /// <summary>
        /// Dequeues work item.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Work item.</returns>
        public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _workItems.TryDequeue(out var workItem);

            return workItem;
        }
    }

    /// <summary>
    /// Background task queue.
    /// </summary>
    public class BackgroundTaskQueue<TService> : BackgroundTaskQueue, IBackgroundTaskQueue<TService>
    {
    }
}




