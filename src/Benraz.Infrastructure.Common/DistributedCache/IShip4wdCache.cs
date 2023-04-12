using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Common.DistributedCache;

/// <summary>
/// Benraz cache.
/// </summary>
public interface IBenrazCache : IBenrazCache<object>
{
}

/// <summary>
/// Generic Benraz cache.
/// </summary>
public interface IBenrazCache<T>
{
    /// <summary>
    /// Get or set value.
    /// </summary>
    Task<T> GetOrSetValueAsync(string key, Func<Task<T>> valueDelegate, DistributedCacheEntryOptions options = null);

    /// <summary>
    /// Is value cached.
    /// </summary>
    Task<bool> IsValueCachedAsync(string key);

    /// <summary>
    /// Get value.
    /// </summary>
    Task<T> GetValueAsync(string key);

    /// <summary>
    /// Set value.
    /// </summary>
    Task SetValueAsync(string key, T value, DistributedCacheEntryOptions options = null);

    /// <summary>
    /// Remove value.
    /// </summary>
    Task RemoveValueAsync(string key);
}


