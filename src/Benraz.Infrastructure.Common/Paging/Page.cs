using System.Collections.Generic;

namespace Benraz.Infrastructure.Common.Paging
{
    /// <summary>
    /// Page of data items.
    /// </summary>
    /// <typeparam name="T">Data item type.</typeparam>
    public class Page<T>
    {
        /// <summary>
        /// Items.
        /// </summary>
        public ICollection<T> Items { get; set; }

        /// <summary>
        /// Page number.
        /// </summary>
        public int PageNo { get; set; }

        /// <summary>
        /// Page size.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total items count.
        /// </summary>
        public int TotalCount { get; set; }
    }
}




