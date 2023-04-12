using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Benraz.Infrastructure.Common.DataRedundancy;

namespace Benraz.Infrastructure.Web.Filters
{
    /// <summary>
    /// Data redundancy action filter attribute.
    /// </summary>
    public class DRFilterAttribute : ActionFilterAttribute
    {
        private readonly IDrChecker _drChecker;

        /// <summary>
        /// Creates attribute.
        /// </summary>
        /// <param name="drChecker">Data redundancy checker.</param>
        public DRFilterAttribute(IDrChecker drChecker)
        {
            _drChecker = drChecker;
        }

        /// <summary>
        /// On action executing.
        /// </summary>
        /// <param name="context">Context.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isActiveDR = _drChecker.IsActiveDR();
            if (!isActiveDR)
            {
                context.Result = new BadRequestObjectResult("DR is inactive.");
            }
        }
    }
}




