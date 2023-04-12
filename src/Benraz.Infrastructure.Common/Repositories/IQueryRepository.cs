using Benraz.Infrastructure.Common.EntityBase;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.Common.Repositories
{
    /// <summary>
    /// Query-only repository.
    /// </summary>
    /// <typeparam name="TKey">Key.</typeparam>
    /// <typeparam name="TEntity">Entity.</typeparam>
    public interface IQueryRepository<TKey, TEntity>
        where TEntity : IEntity<TKey>
    {
        /// <summary>
        /// Returns all entities.
        /// </summary>
        /// <returns>Entities.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Returns entity by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Entity.</returns>
        Task<TEntity> GetByIdAsync(TKey id);
    }
}




