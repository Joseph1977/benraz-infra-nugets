using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Benraz.Infrastructure.Common.Helpers;
using System;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Common.DistributedCache;

/// <summary>
/// Generic distributed cache.
/// </summary>
public abstract class DistributedCache<T> : IDistributedCache<T> where T : class
{
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<IBenrazDistributedCacheWrapper> _logger;

    /// <summary>
    /// Distributed Cache.
    /// </summary>
    protected DistributedCache(IDistributedCache distributedCache, IConfiguration configuration, ILogger<IBenrazDistributedCacheWrapper> logger)
    {
        _distributedCache = distributedCache;
        _logger = logger;
    }

    /// <summary>
    /// Get a value from the cache, or by calling an asynchronous delegate and then caching the value.
    /// </summary>
    public virtual async Task<T> GetOrSetValueAsync(string key, Func<Task<T>> valueDelegate,
        DistributedCacheEntryOptions options)
    {
        var value = await GetValueAsync(key);
        if (value == null)
        {
            //not in cache, get a value, calling delegate
            value = await valueDelegate();

            if (value != null)
                await SetValueAsync(key, value, options ?? GetDefaultOptions());
        }

        return value;
    }

    /// <summary>
    /// Is value cached.
    /// </summary>
    public virtual async Task<bool> IsValueCachedAsync(string key)
    {
        var value = await _distributedCache.GetStringAsync(key);

        return value != null;

    }
    
    /// <summary>
    /// Get value.
    /// </summary>
    public virtual async Task<T> GetValueAsync(string key)
    {
        try
        {
            var value = await _distributedCache.GetAsync(key);
            if (value != null)
            {
                if (typeof(T) == typeof(object))
                {
                    return GZipHelper.UnZipStr(value) as T;
                }
                else
                {
                    return JsonConvert.DeserializeObject<T>(GZipHelper.UnZipStr(value));
                }
            }
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, $"key = {key} , Exception={ex.ToString()}");
        }
        return null;


    }

    /// <summary>
    /// Set value.
    /// </summary>
    public virtual async Task SetValueAsync(string key, T value, DistributedCacheEntryOptions options = null)
    {
        await _distributedCache.SetAsync(key, GZipHelper.ZipStr(JsonConvert.SerializeObject(value)),
            options ?? GetDefaultOptions());
    }

    /// <summary>
    /// Remove value.
    /// </summary>
    public virtual async Task RemoveValueAsync(string key)
    {
        await _distributedCache.RemoveAsync(key);

    }

    /// <summary>
    /// Get default options.
    /// </summary>
    protected abstract DistributedCacheEntryOptions GetDefaultOptions();
}


