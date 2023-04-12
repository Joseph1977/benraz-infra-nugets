namespace Benraz.Infrastructure.Common.DistributedCache;

/// <summary>
/// Distributed cache wrapper.
/// </summary>
public interface IBenrazDistributedCacheWrapper
{
    /// <summary>
    /// Cache.
    /// </summary>
    BenrazCache Cache { get; }
}


