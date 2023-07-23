using Elmah.Io.AspNetCore;
using System.Net;

namespace ProSupplier.Api.Extentions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpcontext)
        {
            try
            {
                await _next(httpcontext);
            }
            catch (Exception ex)
            {

                HandleExceptionAsync(httpcontext, ex);
            }
        }

        private static void HandleExceptionAsync(HttpContext context, Exception exception)
        {
            //await exception.Ship();
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}
