using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NetParts.Libraries.Filter
{
    public class ValidateHttpRefererAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            string referer = context.HttpContext.Request.Headers["Referer"].ToString();
            if (string.IsNullOrEmpty(referer))
            {
                context.Result = new StatusCodeResult(403);
            }
            else
            {
                Uri uri = new Uri(referer);

                string hostReferer = uri.Host;
                string hostServer = context.HttpContext.Request.Host.Host;

                if (hostReferer != hostServer)
                {
                    context.Result = new StatusCodeResult(403);
                }
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {


        }
    }
}
