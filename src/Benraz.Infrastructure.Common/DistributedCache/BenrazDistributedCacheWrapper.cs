using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Benraz.Infrastructure.Common.DistributedCache;

/// <summary>
/// Distributed cache wrapper.
/// </summary>
public class BenrazDistributedCacheWrapper : IBenrazDistributedCacheWrapper
{
    private readonly IConfiguration _configuration;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<IBenrazDistributedCacheWrapper> _logger;
    private BenrazCache _cache;

    /// <summary>
    /// Distributed cache wrapper.
    /// </summary>
    public BenrazDistributedCacheWrapper(IDistributedCache distributedCache, IConfiguration configuration, ILogger<IBenrazDistributedCacheWrapper> logger)
    {
        _configuration = configuration;
        _distributedCache = distributedCache;
        _logger = logger;
    }

    /// <summary>
    /// Cache.
    /// </summary>
    public BenrazCache Cache => _cache ??= new BenrazCache(_distributedCache, _configuration, _logger);
}


