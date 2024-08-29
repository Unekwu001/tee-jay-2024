using dotenv.net;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace Core.EmailServices
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(ILogger<EmailService> logger)
        {
            _logger = logger;
        }

        public async Task SendEmailAsync(string[] recipients, string subject, string body)
        {
            DotEnv.Load(new DotEnvOptions(probeForEnv: false, envFilePaths: new[] { Path.Combine(Directory.GetCurrentDirectory(), ".env") }));

            var smtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER");
            var smtpPort = Environment.GetEnvironmentVariable("SMTP_PORT");
            var smtpEmail = Environment.GetEnvironmentVariable("SMTP_EMAIL");
            var smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD");

            _logger.LogInformation($"SMTP_SERVER: {smtpServer}, SMTP_PORT: {smtpPort}, SMTP_EMAIL: {smtpEmail}");

            if (string.IsNullOrEmpty(smtpServer) || string.IsNullOrEmpty(smtpPort) ||
                string.IsNullOrEmpty(smtpEmail) || string.IsNullOrEmpty(smtpPassword))
            {
                _logger.LogError("One or more SMTP environment variables are not set.");
                throw new Exception("SMTP environment variables are not set.");
            }

            try
            {
                using var smtpClient = new SmtpClient(smtpServer)
                {
                    Port = int.Parse(smtpPort),
                    Credentials = new NetworkCredential(smtpEmail, smtpPassword),
                    EnableSsl = true,
                };

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };

                foreach (var recipient in recipients)
                {
                    mailMessage.To.Add(recipient);
                }

                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation("Email sent successfully.");
            }
            catch (SmtpException ex)
            {
                _logger.LogError(ex, "Error sending email.");
                throw new Exception("Error sending email.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while sending email.");
                throw new Exception("Unexpected error occurred while sending email.", ex);
            }
        }
    }
}