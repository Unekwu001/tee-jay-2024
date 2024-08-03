using Data.AppDbContext;
using Data.Models;
using Microsoft.Extensions.DependencyInjection;
namespace Data.Seed
{ 
    public static class EmailSettingsInitializer
    {
        public static void SeedEmailSettings(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TeejayDbContext>();

            if (!context.EmailSettings.Any())
            {
                context.EmailSettings.Add(new EmailSettings
                {
                    SmtpServer = "smtp.outlook.com",
                    Port = 587,
                    SenderEmail = "DivineOgbere@outlook.com",
                    SenderPassword = "Deevyn-2005"
                });
                context.SaveChanges();
            }
        }
    }

}
