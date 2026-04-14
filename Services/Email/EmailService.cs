using System.Net;
using System.Net.Mail;

namespace GestionAbsences.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var smtpServer = _configuration["Email:SmtpServer"] ?? "localhost";
                var smtpPort = int.TryParse(_configuration["Email:SmtpPort"], out var port) ? port : 587;
                var senderEmail = _configuration["Email:SenderEmail"] ?? "noreply@gestionabsences.com";
                var senderName = _configuration["Email:SenderName"] ?? "Gestion Absences";
                var password = _configuration["Email:Password"] ?? string.Empty;

                using var client = new SmtpClient(smtpServer, smtpPort)
                {
                    Credentials = new NetworkCredential(senderEmail, password),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, senderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(to);

                await client.SendMailAsync(mailMessage);
                _logger.LogInformation("Email sent successfully to {To}", to);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {To}", to);
            }
        }
    }
}
