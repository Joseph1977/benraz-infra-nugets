using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;

namespace Benraz.Infrastructure.Common.Http
{
    /// <summary>
    /// Query builder.
    /// </summary>
    public class QueryBuilder
    {
        private const string DEFAULT_DATE_FORMAT = "yyyy-MM-ddTHH:mm:sszzz";

        private ICollection<KeyValuePair<string, string>> _queryParameters;

        /// <summary>
        /// Creates builder.
        /// </summary>
        public QueryBuilder()
        {
            _queryParameters = new List<KeyValuePair<string, string>>();
        }

        /// <summary>
        /// Adds query parameter.
        /// </summary>
        /// <param name="key">Parameter key.</param>
        /// <param name="value">Parameter value.</param>
        /// <returns>Query builder.</returns>
        public QueryBuilder AddQueryParameter(string key, string value)
        {
            _queryParameters.Add(new KeyValuePair<string, string>(key, value));
            return this;
        }

        /// <summary>
        /// Adds query parameter.
        /// </summary>
        /// <param name="key">Parameter key.</param>
        /// <param name="value">Parameter value.</param>
        /// <returns>Query builder.</returns>
        public QueryBuilder AddQueryParameter(string key, decimal? value)
        {
            return AddQueryParameter(key, value?.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Adds query parameter.
        /// </summary>
        /// <param name="key">Parameter key.</param>
        /// <param name="value">Parameter value.</param>
        /// <param name="format">Date and time format.</param>
        /// <returns>Query builder.</returns>
        public QueryBuilder AddQueryParameter(string key, DateTime? value, string format = DEFAULT_DATE_FORMAT)
        {
            return AddQueryParameter(key, value?.ToString(format));
        }

        /// <summary>
        /// Adds query parameter.
        /// </summary>
        /// <param name="key">Parameter key.</param>
        /// <param name="value">Parameter value.</param>
        /// <returns>Query builder.</returns>
        public QueryBuilder AddQueryParameter(string key, object value)
        {
            return AddQueryParameter(key, value?.ToString());
        }

        /// <summary>
        /// Adds query parameter.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="key">Parameter key.</param>
        /// <param name="values">Parameter values.</param>
        /// <returns>Query builder.</returns>
        public QueryBuilder AddQueryParameter<T>(string key, IEnumerable<T> values)
        {
            values = values ?? new List<T>();
            foreach (var value in values)
            {
                AddQueryParameter(key, value);
            }

            return this;
        }

        /// <summary>
        /// Returns query string.
        /// </summary>
        /// <returns>Query string.</returns>
        public override string ToString()
        {
            var query = GetQuery();
            return query;
        }

        private string GetQuery()
        {
            var queryParameterPairs = new List<string>();
            foreach (var queryParameter in _queryParameters)
            {
                if (!string.IsNullOrEmpty(queryParameter.Value))
                {
                    var key = WebUtility.UrlEncode(queryParameter.Key);
                    var value = WebUtility.UrlEncode(queryParameter.Value);
                    queryParameterPairs.Add($"{key}={value}");
                }
            }

            return string.Join("&", queryParameterPairs);
        }
    }
}




