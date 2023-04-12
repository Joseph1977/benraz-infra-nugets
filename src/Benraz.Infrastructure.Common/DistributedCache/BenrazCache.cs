using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace Benraz.Infrastructure.Common.DistributedCache;

/// <summary>
/// Generic Benraz cache.
/// </summary>
public class BenrazCache<T> : DistributedCache<T>, IBenrazCache<T>
    where T : class
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Benraz cache.
    /// </summary>
    public BenrazCache(IDistributedCache distributedCache,
        IConfiguration configuration, ILogger<IBenrazDistributedCacheWrapper> logger)
        : base(distributedCache, configuration, logger)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Get a value from the cache, or by calling an asynchronous delegate and then caching the value
    /// </summary> 
    protected override DistributedCacheEntryOptions GetDefaultOptions()
    {
        if (!TimeSpan.TryParseExact(_configuration["DistributedCache:AbsoluteExpiration"], "hh\\:mm\\:ss",
                System.Globalization.CultureInfo.InvariantCulture, out var absoluteExpiration))
        {
            absoluteExpiration = new TimeSpan(1, 0, 0);
        }

        if (!TimeSpan.TryParseExact(_configuration["DistributedCache:SlidingExpiration"], "hh\\:mm\\:ss",
                System.Globalization.CultureInfo.InvariantCulture, out var slidingExpiration))
        {
            slidingExpiration = new TimeSpan(0, 5, 0);
        }

        //use default caching options for the class if they are not defined in options parameter
        return new DistributedCacheEntryOptions()
        {
            AbsoluteExpiration = DateTime.UtcNow.AddHours(absoluteExpiration.TotalHours),
            SlidingExpiration = slidingExpiration
        };
    }
}

/// <summary>
/// Benraz cache.
/// </summary>
public class BenrazCache : BenrazCache<object>, IBenrazCache
{
    /// <summary>
    /// Benraz cache.
    /// </summary>
    public BenrazCache(IDistributedCache distributedCache, IConfiguration configuration, ILogger<IBenrazDistributedCacheWrapper> logger)
        : base(distributedCache, configuration, logger)
    {
    }
}


