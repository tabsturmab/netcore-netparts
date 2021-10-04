using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace NetParts.Libraries.Middleware
{
    public class ValidateAntiForgeryTokenMiddleware
    {
        private RequestDelegate _next;
        private IAntiforgery _antiforgery;

        public ValidateAntiForgeryTokenMiddleware(RequestDelegate next, IAntiforgery antiforgery)
        {
            _next = next;
            _antiforgery = antiforgery;
        }
        public async Task Invoke(HttpContext context)
        {
            var Header = context.Request.Headers["x-requested-with"];
            bool AJAX = (Header == "XMLHttpRequest") ? true : false;

            if (HttpMethods.IsPost(context.Request.Method) && !(context.Request.Form.Files.Count == 1 && AJAX))
            {
                await _antiforgery.ValidateRequestAsync(context);
            }
            await _next(context);
        }
    }
}

