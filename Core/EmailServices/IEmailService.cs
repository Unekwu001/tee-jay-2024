
namespace Core.EmailServices
{
    public interface IEmailService
    {
        Task SendEmailAsync(string[] recipients, string subject, string body);
    }
}