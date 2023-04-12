using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;

namespace Benraz.Infrastructure.Common.Http
{
    /// <summary>
    /// Endpoint builder.
    /// </summary>
    public class EndpointBuilder
    {
        private const string DEFAULT_DATE_FORMAT = "yyyy-MM-ddTHH:mm:sszzz";

        private string _baseEndpoint;
        private ICollection<string> _segments;
        private ICollection<KeyValuePair<string, string>> _queryParameters;

        /// <summary>
        /// Creates builder.
        /// </summary>
        public EndpointBuilder()
        {
            _queryParameters = new List<KeyValuePair<string, string>>();
            _segments = new List<string>();
        }

        /// <summary>
        /// Creates builder.
        /// </summary>
        /// <param name="baseEndpoint">Base endpoint.</param>
        public EndpointBuilder(string baseEndpoint)
            : this()
        {
            _baseEndpoint = baseEndpoint;
        }

        /// <summary>
        /// Sets base endpoint.
        /// </summary>
        /// <param name="baseEndpoint">Base endpoint.</param>
        /// <returns>Endpoint builder.</returns>
        public EndpointBuilder SetBaseEndpoint(string baseEndpoint)
        {
            _baseEndpoint = baseEndpoint;
            return this;
        }

        /// <summary>
        /// Adds query parameter.
        /// </summary>
        /// <param name="key">Parameter key.</param>
        /// <param name="value">Parameter value.</param>
        /// <returns>Endpoint builder.</returns>
        public EndpointBuilder AddQueryParameter(string key, string value)
        {
            _queryParameters.Add(new KeyValuePair<string, string>(key, value));
            return this;
        }

        /// <summary>
        /// Adds query parameter.
        /// </summary>
        /// <param name="key">Parameter key.</param>
        /// <param name="value">Parameters values.</param>
        /// <returns>Endpoint builder.</returns>
        public EndpointBuilder AddProfileQueryParameter(string key, dynamic value)
        {
            _queryParameters.Add(new KeyValuePair<string, string>($"{key}.userId", value?.UserId?.ToString()));
            _queryParameters.Add(new KeyValuePair<string, string>($"{key}.organizationId", value?.OrganizationId?.ToString()));
            _queryParameters.Add(new KeyValuePair<string, string>($"{key}.userName", value?.UserName?.ToString()));
            _queryParameters.Add(new KeyValuePair<string, string>($"{key}.organizationName", value?.OrganizationName?.ToString()));
            _queryParameters.Add(new KeyValuePair<string, string>($"{key}.isAgent", value?.IsAgent?.ToString()));
            _queryParameters.Add(new KeyValuePair<string, string>($"{key}.orgKycStatus", value?.OrgKycStatus?.ToString()));
            _queryParameters.Add(new KeyValuePair<string, string>($"{key}.createTimeUtc", value?.CreateTimeUtc?.ToString(DEFAULT_DATE_FORMAT)));
            return this;
        }

        /// <summary>
        /// Adds query parameter.
        /// </summary>
        /// <param name="key">Parameter key.</param>
        /// <param name="value">Parameter value.</param>
        /// <returns>Endpoint builder.</returns>
        public EndpointBuilder AddQueryParameter(string key, decimal? value)
        {
            return AddQueryParameter(key, value?.ToString(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Adds query parameter.
        /// </summary>
        /// <param name="key">Parameter key.</param>
        /// <param name="value">Parameter value.</param>
        /// <param name="format">Date and time format.</param>
        /// <returns>Endpoint builder.</returns>
        public EndpointBuilder AddQueryParameter(string key, DateTime? value, string format = DEFAULT_DATE_FORMAT)
        {
            return AddQueryParameter(key, value?.ToString(format));
        }

        /// <summary>
        /// Adds query parameter.
        /// </summary>
        /// <param name="key">Parameter key.</param>
        /// <param name="value">Parameter value.</param>
        /// <returns>Endpoint builder.</returns>
        public EndpointBuilder AddQueryParameter(string key, object value)
        {
            return AddQueryParameter(key, value?.ToString());
        }

        /// <summary>
        /// Adds query parameter.
        /// </summary>
        /// <typeparam name="T">Value type.</typeparam>
        /// <param name="key">Parameter key.</param>
        /// <param name="values">Parameter values.</param>
        /// <returns>Endpoint builder.</returns>
        public EndpointBuilder AddQueryParameter<T>(string key, IEnumerable<T> values)
        {
            values = values ?? new List<T>();
            foreach (var value in values)
            {
                AddQueryParameter(key, value);
            }

            return this;
        }

        /// <summary>
        /// Adds segment.
        /// </summary>
        /// <param name="segment">Segment.</param>
        /// <returns>Endpoint builder.</returns>
        public EndpointBuilder AddSegment(string segment)
        {
            if (!string.IsNullOrEmpty(segment))
            {
                _segments.Add(segment);
            }
            return this;
        }

        /// <summary>
        /// Adds segment.
        /// </summary>
        /// <param name="segment">Segment.</param>
        /// <returns>Endpoint builder.</returns>
        public EndpointBuilder AddSegment(object segment)
        {
            return AddSegment(segment?.ToString());
        }

        /// <summary>
        /// Returns endpoint with query string.
        /// </summary>
        /// <returns>Endpoint.</returns>
        public override string ToString()
        {
            var endpoint = _baseEndpoint;

            var segments = GetSegments();
            if (!string.IsNullOrEmpty(segments))
            {
                endpoint += $"/{segments}";
            }

            var query = GetQuery();
            if (!string.IsNullOrEmpty(query))
            {
                endpoint += $"?{query}";
            }

            return TrimStartSlash(endpoint);
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

        private string GetSegments()
        {
            return string.Join("/", _segments);
        }

        private string TrimStartSlash(string url)
        {
            return url.TrimStart(new[] { '/' });
        }
    }
}




