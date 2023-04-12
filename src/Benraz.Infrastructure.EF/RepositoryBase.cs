using Microsoft.EntityFrameworkCore;
using Benraz.Infrastructure.Common.EntityBase;
using Benraz.Infrastructure.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.EF
{
    /// <summary>
    /// Base Entity Framework repository.
    /// </summary>
    /// <typeparam name="TKey">Key.</typeparam>
    /// <typeparam name="TEntity">Entity.</typeparam>
    /// <typeparam name="TContext">Context.</typeparam>
    public abstract class RepositoryBase<TKey, TEntity, TContext> :
        QueryRepositoryBase<TKey, TEntity, TContext>,
        IRepository<TKey, TEntity>
        where TEntity : class, IAggregateRoot<TKey>
        where TContext : DbContext
    {
        /// <summary>
        /// Creates repository.
        /// </summary>
        /// <param name="context">Context.</param>
        public RepositoryBase(TContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Adds entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <returns>Task.</returns>
        public virtual Task AddAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            PrepareAdd(entity);
            Context.Add(entity);

            return Context.SaveChangesAsync();
        }

        /// <summary>
        /// Adds entities.
        /// </summary>
        /// <param name="entities">Entities.</param>
        /// <returns>Task.</returns>
        public virtual Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            foreach (var entity in entities)
            {
                PrepareAdd(entity);
            }

            Context.AddRange(entities);
            return Context.SaveChangesAsync();
        }

        /// <summary>
        /// Changes entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <returns>Task.</returns>
        public virtual Task ChangeAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            PrepareChange(entity);
            Context.Update(entity);

            return Context.SaveChangesAsync();
        }

        /// <summary>
        /// Changes entities.
        /// </summary>
        /// <param name="entities">Entities.</param>
        /// <returns>Task.</returns>
        public virtual Task ChangeRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            foreach (var entity in entities)
            {
                PrepareChange(entity);
            }

            Context.UpdateRange(entities);
            return Context.SaveChangesAsync();
        }

        /// <summary>
        /// Removes entity.
        /// </summary>
        /// <param name="entity">Entity.</param>
        /// <returns>Task.</returns>
        public virtual Task RemoveAsync(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Context.Remove(entity);
            return Context.SaveChangesAsync();
        }

        /// <summary>
        /// Removes entities.
        /// </summary>
        /// <param name="entities">Entities.</param>
        /// <returns>Task.</returns>
        public virtual Task RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            Context.RemoveRange(entities);
            return Context.SaveChangesAsync();
        }

        /// <summary>
        /// Prepares entity for add.
        /// </summary>
        /// <param name="entity">Entity.</param>
        protected virtual void PrepareAdd(TEntity entity)
        {
            entity.CreateTimeUtc = DateTime.UtcNow;
            entity.UpdateTimeUtc = DateTime.UtcNow;
        }

        /// <summary>
        /// Prepapres entity for change.
        /// </summary>
        /// <param name="entity">Entity.</param>
        protected virtual void PrepareChange(TEntity entity)
        {
            entity.UpdateTimeUtc = DateTime.UtcNow;
        }
    }
}




