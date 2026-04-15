namespace FinanceTracker.IService
{
    public interface IEmailService
    {
        Task<bool> SendEmail(string toEmail, int userId, string token);
    }
}
