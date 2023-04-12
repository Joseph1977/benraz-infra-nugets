using System;

namespace Benraz.Infrastructure.Common.EntityBase
{
    /// <summary>
    /// Aggregate root.
    /// </summary>
    /// <typeparam name="TKey">Key.</typeparam>
    public interface IAggregateRoot<TKey> : IEntity<TKey>
    {
        /// <summary>
        /// Create time in UTC.
        /// </summary>
        DateTime CreateTimeUtc { get; set; }

        /// <summary>
        /// Last update time in UTC.
        /// </summary>
        DateTime UpdateTimeUtc { get; set; }
    }
}


