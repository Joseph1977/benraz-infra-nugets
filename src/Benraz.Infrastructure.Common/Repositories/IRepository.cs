using Benraz.Infrastructure.Common.EntityBase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Common.Repositories
{
    /// <summary>
    /// Repository.
    /// </summary>
    /// <typeparam name="TKey">Key.</typeparam>
    /// <typeparam name="TEntity">Entity.</typeparam>
    public interface IRepository<TKey, TEntity> : IQueryRepository<TKey, TEntity>
        where TEntity : IAggregateRoot<TKey>
    {
        /// <summary>
        /// Adds entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <returns>Task.</returns>
        Task AddAsync(TEntity entity);

        /// <summary>
        /// Adds entities.
        /// </summary>
        /// <param name="entities">Entities.</param>
        /// <returns>Task.</returns>
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Changes entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <returns>Task.</returns>
        Task ChangeAsync(TEntity entity);

        /// <summary>
        /// Changes entities.
        /// </summary>
        /// <param name="entities">Entities.</param>
        /// <returns>Task.</returns>
        Task ChangeRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Removes entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <returns>Task.</returns>
        Task RemoveAsync(TEntity entity);

        /// <summary>
        /// Removes entities.
        /// </summary>
        /// <param name="entities">Entities.</param>
        /// <returns>Task.</returns>
        Task RemoveRangeAsync(IEnumerable<TEntity> entities);
    }
}




