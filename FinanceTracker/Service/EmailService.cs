using FinanceTracker.IService;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;


namespace FinanceTracker.Service
{


    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmail(string toEmail, int userId, string token)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(
                emailSettings["SenderName"],
                emailSettings["SenderEmail"]));

            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = "Verify Your Email";

            var frontendUrl = _configuration["FrontendUrl"];
            var verificationLink = $"{frontendUrl}/verify?email={toEmail}&token={Uri.EscapeDataString(token)}";

            message.Body = new TextPart("html")
            {
                Text = $"<h2>Email Verification</h2>" +
                       $"<p>Please click the link below to verify your email: </p>" +
                       $"<a href='{verificationLink}'>Click Here To Verify</a>"
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(
                emailSettings["SmtpServer"],
                int.Parse(emailSettings["Port"]),
                SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(
                emailSettings["SenderEmail"],
                emailSettings["Password"]);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            return true;
        }




    }
}
