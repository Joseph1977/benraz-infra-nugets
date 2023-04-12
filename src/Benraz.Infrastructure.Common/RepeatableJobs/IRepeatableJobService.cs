using System.Threading.Tasks;

namespace Benraz.Infrastructure.Common.RepeatableJobs
{
    /// <summary>
    /// Repeatable jobs service.
    /// </summary>
    public interface IRepeatableJobService
    {
        /// <summary>
        /// Returns if execution is in progress.
        /// </summary>
        /// <param name="args">Arguments.</param>
        /// <returns>If execution is in progress.</returns>
        Task<bool> IsInProcessingAsync(object args = null);

        /// <summary>
        /// Returns if execution is on cooldown.
        /// </summary>
        /// <param name="args">Arguments.</param>
        /// <returns>If execution is on cooldown.</returns>
        Task<bool> IsOnCooldownAsync(object args = null);

        /// <summary>
        /// Cleans up stale jobs.
        /// </summary>
        /// <param name="args">Arguments.</param>
        /// <returns>Task.</returns>
        Task CleanUpStaleAsync(object args = null);

        /// <summary>
        /// Processes new job.
        /// </summary>
        /// <param name="args">Arguments.</param>
        /// <returns>Task.</returns>
        Task ProcessAsync(object args = null);
    }
}




