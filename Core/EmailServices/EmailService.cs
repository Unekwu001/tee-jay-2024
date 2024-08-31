using Core.EmailServices;
using dotenv.net;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly SmtpClient _smtpClient;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;

        DotEnv.Load(new DotEnvOptions(probeForEnv: false, envFilePaths: new[] { Path.Combine(Directory.GetCurrentDirectory(), ".env") }));

        var smtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER");
        var smtpPort = Environment.GetEnvironmentVariable("SMTP_PORT");
        var smtpEmail = Environment.GetEnvironmentVariable("SMTP_EMAIL");
        var smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD");

        if (string.IsNullOrEmpty(smtpServer) || string.IsNullOrEmpty(smtpPort) ||
            string.IsNullOrEmpty(smtpEmail) || string.IsNullOrEmpty(smtpPassword))
        {
            _logger.LogError("One or more SMTP environment variables are not set.");
            throw new InvalidOperationException("SMTP environment variables are not set.");
        }

        _smtpClient = new SmtpClient(smtpServer)
        {
            Port = int.Parse(smtpPort),
            Credentials = new NetworkCredential(smtpEmail, smtpPassword),
            EnableSsl = true,
        };
    }

    public async Task SendEmailAsync(string[] recipients, string subject, string body)
    {
        foreach (var recipient in recipients)
        {
            using var mailMessage = new MailMessage
            {
                From = new MailAddress(Environment.GetEnvironmentVariable("SMTP_EMAIL")),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(recipient);

            try
            {
                await _smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation($"Email sent to {recipient}.");
                // Delay to avoid hitting rate limits
                await Task.Delay(500); // 500ms delay, adjust as needed
            }
            catch (SmtpException ex)
            {
                _logger.LogError(ex, $"SMTP error occurred while sending email to {recipient}.");
                // Handle specific recipient errors if needed
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error occurred while sending email to {recipient}.");
                // Handle specific recipient errors if needed
            }
        }
    }
}
