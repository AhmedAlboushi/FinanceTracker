
namespace FinanceTracker.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = ex switch
            {
                InvalidOperationException => 409,
                KeyNotFoundException => 404,
                UnauthorizedAccessException => 403,
                _ => 500
            };

            var message = ex.InnerException?.Message ?? ex.Message;
            return context.Response.WriteAsJsonAsync(new { error = message });
        }
    }
}
