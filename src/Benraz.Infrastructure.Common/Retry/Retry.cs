using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Common.Retry;

/// <summary>
/// Retry.
/// </summary>
public static class Retry
{
    /// <summary>
    /// Do.
    /// </summary>
    /// <param name="task">Task.</param>
    /// <param name="retryInterval">Retry interval.</param>
    /// <param name="maxAttemptCount">Max attempt count.</param>
    public static async Task Do(Func<Task> task, TimeSpan retryInterval, int maxAttemptCount = 3)
    {
        var exceptions = new List<Exception>();
        for (int attempted = 0; attempted < maxAttemptCount; attempted++)
        {
            try
            {
                if (attempted > 0)
                {
                    await Task.Delay(retryInterval);
                }

                await task();
                return;
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
        }
        throw new AggregateException(exceptions);
    }

    /// <summary>
    /// Do.
    /// </summary>
    /// <param name="task">Task.</param>
    /// <param name="retryInterval">Retry interval.</param>
    /// <param name="maxAttemptCount">Max attempt count.</param>
    /// <returns>T</returns>
    /// <exception cref="AggregateException"></exception>
    public static async Task<T> Do<T>(Func<Task<T>> task, TimeSpan retryInterval, int maxAttemptCount = 3)
    {
        var exceptions = new List<Exception>();
        for (int attempted = 0; attempted < maxAttemptCount; attempted++)
        {
            try
            {
                if (attempted > 0)
                {
                    await Task.Delay(retryInterval);
                }
                return await task();
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
        }
        throw new AggregateException(exceptions);
    }
}


