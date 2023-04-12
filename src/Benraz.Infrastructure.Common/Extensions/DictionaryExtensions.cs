using System;
using System.Collections.Generic;

namespace Benraz.Infrastructure.Common.Extensions
{
    /// <summary>
    /// Dictionary extensions.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Returns value by key if value exists.
        /// </summary>
        /// <typeparam name="TKey">Key type.</typeparam>
        /// <typeparam name="TValue">Value type.</typeparam>
        /// <param name="dictionary">Dictionary.</param>
        /// <param name="key">Key.</param>
        /// <returns>Value.</returns>
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey? key)
            where TKey : struct
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (!key.HasValue)
            {
                return default;
            }

            return dictionary.TryGetValue(key.Value, out var value) ? value : default;
        }
    }
}




