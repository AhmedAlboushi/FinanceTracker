using FinanceTracker.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FinanceTracker.Filters
{
    public class LogActionAttribute : ActionFilterAttribute
    {

        private readonly string _action;
        private readonly byte _level;
        public LogActionAttribute(string action, byte level)
        {
            _action = action;
            _level = level;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var executed = await next();

            // The Exception Middlware wraps the attributes so it will return the exception first and not the status code
            // if we dont do this it will always give 200 status code default
            short statusCode;
            if (executed.Exception != null)
            {
                statusCode = (short)(executed.Exception switch
                {
                    InvalidOperationException => 409,
                    KeyNotFoundException => 404,
                    UnauthorizedAccessException => 403,
                    _ => 500
                });
            }
            else
            {
                statusCode = (short)context.HttpContext.Response.StatusCode;
            }

            await LogHelper.WriteLog(context.HttpContext, _action, statusCode, _level);
        }
    }
}
