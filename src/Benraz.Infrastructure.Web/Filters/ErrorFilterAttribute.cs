using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Benraz.Infrastructure.Common.Exceptions;
using System;
using System.Net;

namespace Benraz.Infrastructure.Web.Filters
{
    /// <summary>
    /// Error filter attribute.
    /// </summary>
    public class ErrorFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<ErrorFilterAttribute> _logger;

        /// <summary>
        /// Creates attribute.
        /// </summary>
        /// <param name="logger">Logger.</param>
        public ErrorFilterAttribute(ILogger<ErrorFilterAttribute> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// On exception.
        /// </summary>
        /// <param name="context">Exception context.</param>
        public override void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "An error has occured.");

            context.ExceptionHandled = true;
            context.Result = new ObjectResult(GetMessage(context.Exception));
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }

        private string GetMessage(Exception exception)
        {
            var domainException = exception as DomainException;
            if (!string.IsNullOrEmpty(domainException?.UserMessage))
            {
                return domainException.UserMessage;
            }

            return "Failed to process operation.";
        }
    }
}




