using System;

namespace Benraz.Infrastructure.Common.EntityBase
{
    /// <summary>
    /// Aggregate root.
    /// </summary>
    /// <typeparam name="TKey">Key.</typeparam>
    public abstract class AggregateRootBase<TKey> : IAggregateRoot<TKey>
    {
        /// <summary>
        /// Creates aggregate root base.
        /// </summary>
        protected AggregateRootBase()
        {
            CreateTimeUtc = DateTime.UtcNow;
            UpdateTimeUtc = CreateTimeUtc;
        }

        /// <summary>
        /// Identifier.
        /// </summary>
        public TKey Id { get; set; }

        /// <summary>
        /// Create time in UTC.
        /// </summary>
        public DateTime CreateTimeUtc { get; set; }

        /// <summary>
        /// Last update time in UTC.
        /// </summary>
        public DateTime UpdateTimeUtc { get; set; }
    }
}


