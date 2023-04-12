using Microsoft.EntityFrameworkCore;
using Benraz.Infrastructure.Common.EntityBase;
using Benraz.Infrastructure.Common.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.EF
{
    /// <summary>
    /// Base Entity Framework repository with retrieval support.
    /// </summary>
    /// <typeparam name="TKey">Key.</typeparam>
    /// <typeparam name="TEntity">Entity.</typeparam>
    /// <typeparam name="TContext">Context.</typeparam>
    public abstract class QueryRepositoryBase<TKey, TEntity, TContext> : IQueryRepository<TKey, TEntity>
        where TEntity : class, IEntity<TKey>
        where TContext : DbContext
    {
        /// <summary>
        /// Context.
        /// </summary>
        protected TContext Context { get; private set; }

        /// <summary>
        /// Creates repository.
        /// </summary>
        /// <param name="context">Context.</param>
        public QueryRepositoryBase(TContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Returns all entities.
        /// </summary>
        /// <returns>Entities.</returns>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await GetQuery().ToListAsync();
        }

        /// <summary>
        /// Returns entity by identifier.
        /// </summary>
        /// <param name="id">Identifier.</param>
        /// <returns>Entity.</returns>
        public virtual Task<TEntity> GetByIdAsync(TKey id)
        {
            return GetQuery().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        /// <summary>
        /// Returns query.
        /// </summary>
        /// <returns>Query.</returns>
        protected virtual IQueryable<TEntity> GetQuery()
        {
            return Context.Set<TEntity>();
        }
    }
}




