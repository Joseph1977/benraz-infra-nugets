using Microsoft.EntityFrameworkCore;
using Benraz.Infrastructure.Common.Paging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Benraz.Infrastructure.EF
{
    /// <summary>
    /// Queryable extensions.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Returns items page.
        /// </summary>
        /// <typeparam name="T">Items type.</typeparam>
        /// <param name="query">Query.</param>
        /// <param name="pageNo">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>Items page.</returns>
        public static async Task<Page<T>> ToPageAsync<T>(this IQueryable<T> query, int pageNo, int pageSize)
        {
            pageNo = Math.Max(pageNo, 1);
            pageSize = Math.Max(pageSize, 1);

            var totalCount = await query.CountAsync();
            var pageItems = await query.Skip(pageSize * (pageNo - 1)).Take(pageSize).ToListAsync();

            return new Page<T>
            {
                Items = pageItems,
                PageNo = pageNo,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }
    }
}




