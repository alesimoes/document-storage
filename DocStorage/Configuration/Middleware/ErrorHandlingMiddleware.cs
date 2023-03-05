using Newtonsoft.Json;
using System.Net;

namespace DocStorage.Api.Configuration
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.BadRequest;
            if (exception is NullReferenceException) code = HttpStatusCode.NotFound;

            var result = JsonConvert.SerializeObject(new
            {
                Type = exception.GetType().ToString(),
                Title = exception.Message,
                Status = code,
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
