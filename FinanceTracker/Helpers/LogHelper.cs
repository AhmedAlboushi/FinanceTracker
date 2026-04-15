using FinanceTracker.Data;
using FinanceTracker.Models;

namespace FinanceTracker.Helpers
{

    public static class LogHelper
    {
        public static async Task WriteLog(HttpContext context, string action, short statusCode, byte securityLevel)
        {
            // middlware exception doesn't catch this it can fail silently
            try
            {


                var db = context.RequestServices.GetService<FinanceTrackerDbContext>();
                if (db == null) return;

                var userIdClaim = context.User?.FindFirst("UserId")?.Value;

                var log = new Log
                {
                    Userid = userIdClaim != null ? int.Parse(userIdClaim) : null,
                    Action = action,
                    Httpmethod = context.Request.Method,
                    Endpoint = context.Request.Path,
                    Statuscode = statusCode,
                    Securitylevel = securityLevel,
                    Ipaddress = context.Connection.RemoteIpAddress?.ToString(),
                    Createdat = DateTime.Now
                };

                db.Logs.Add(log);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Can be logged into a file
                Console.WriteLine($"Logging failed: {ex.Message}");
            }
        }
    }
}
