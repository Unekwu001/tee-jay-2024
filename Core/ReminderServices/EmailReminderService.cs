using Core.AttendantUserServices;
using Core.EmailServices;
using Data.AppDbContext;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ReminderServices
{

    public class EmailReminderService : IEmailReminderService
    {
        private readonly TeejayDbContext _dbContext;
        private readonly IEmailService _emailService;

        public EmailReminderService(TeejayDbContext dbContext, IEmailService emailService)
        {
            _dbContext = dbContext;
            _emailService = emailService;
        }

        public async Task SendWeeklyRemindersAsync()
        {
            var attendants = await _dbContext.Attendants.Select(a => a.Email).ToArrayAsync();

            var weddingDate = DateTime.Parse("2024-11-1"); // Example wedding date
            var daysLeft = (weddingDate - DateTime.Today).Days;

            await _emailService.SendEmailAsync(attendants, "Wedding Reminder", $"There are {daysLeft} days left until the wedding!");

            //foreach (var attendant in attendants)
            //{
            //    await _emailService.SendEmailAsync(attendant.Email, "Wedding Reminder", $"There are {daysLeft} days left until the wedding!");

            //    // Uncomment and implement SMS service if needed
            //    // await _smsService.SendSmsAsync(attendant.PhoneNumber, $"There are {daysLeft} days left until the wedding!");
            //}
        }
    }

}
