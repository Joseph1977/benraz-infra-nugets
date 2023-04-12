using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Benraz.Infrastructure.Web.Filters
{
    /// <summary>
    /// Logging filter attribute.
    /// </summary>
    public class LoggingFilterAttribute : ActionFilterAttribute
    {
        private const string MESSAGE_PARTS_SEPARATOR = ". ";

        private readonly ILogger<LoggingFilterAttribute> _logger;

        /// <summary>
        /// Creates attribute.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public LoggingFilterAttribute(ILogger<LoggingFilterAttribute> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// On action executed.
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var messageParts = new List<string>();

            var ipAddress = GetIPAddress(context.HttpContext);
            messageParts.Add($"IP address: {ipAddress}");

            var action = GetAction(context.HttpContext);
            messageParts.Add($"Action: {action}");

            var statusCode = GetStatusCode(context);
            if (statusCode.HasValue)
            {
                messageParts.Add($"Status code: {statusCode}");
            }

            var error = GetErrorValue(context);
            if (!string.IsNullOrEmpty(error))
            {
                messageParts.Add($"Error: {error}");
            }

            var message = string.Join(MESSAGE_PARTS_SEPARATOR, messageParts);
            _logger.LogDebug(message);
        }

        private string GetIPAddress(HttpContext httpContext)
        {
            return httpContext.Connection.RemoteIpAddress?.ToString();
        }

        private string GetAction(HttpContext httpContext)
        {
            return $"{httpContext.Request.Method} {httpContext.Request.Path}";
        }

        private int? GetStatusCode(ActionExecutedContext context)
        {
            if (context.Result is StatusCodeResult statusCodeResult)
            {
                return statusCodeResult.StatusCode;
            }

            if (context.Result is ObjectResult objectResult)
            {
                return objectResult.StatusCode;
            }

            return null;
        }

        private string GetErrorValue(ActionExecutedContext context)
        {
            var statusCode = GetStatusCode(context);
            var isSuccessHttpStatusCode = IsSuccessHttpStatusCode(statusCode);

            if (isSuccessHttpStatusCode)
            {
                return string.Empty;
            }

            var result = GetResultValue(context);
            if (result == null)
            {
                return string.Empty;
            }

            return JsonConvert.SerializeObject(result);
        }

        private bool IsSuccessHttpStatusCode(int? statusCode)
        {
            if (!statusCode.HasValue)
            {
                return false;
            }

            return statusCode >= 200 && statusCode < 300;
        }

        private object GetResultValue(ActionExecutedContext context)
        {
            return (context.Result as ObjectResult)?.Value;
        }
    }
}



