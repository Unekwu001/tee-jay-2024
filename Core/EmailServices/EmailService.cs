using System.Net.Mail;
using System.Net;
using Data.AppDbContext;
using Microsoft.EntityFrameworkCore;

namespace Core.EmailServices
{
    public class EmailService : IEmailService
    {
        private readonly TeejayDbContext _context;

        public EmailService(TeejayDbContext context)
        {
            _context = context;
        }

        public async Task SendEmailAsync(string[] recipients, string subject, string body)
        {
            var emailSettings = await _context.EmailSettings
                .OrderBy(e => e.Id) 
                .FirstOrDefaultAsync();

            if (emailSettings == null)
            {
                throw new Exception("Email settings not found");
            }

            var smtpClient = new SmtpClient(emailSettings.SmtpServer)
            {
                Port = emailSettings.Port,
                Credentials = new NetworkCredential(emailSettings.SenderEmail, emailSettings.SenderPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(emailSettings.SenderEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            foreach (var recipient in recipients)
            {
                mailMessage.To.Add(recipient);
            }

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
