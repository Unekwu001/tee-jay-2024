using Core.EmailServices;
using Core.ReminderServices;
using Data.Models;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("/api[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IBackgroundJobClient _jobClient;
        private readonly IEmailService _emailService;
        private readonly IEmailReminderService _emailReminderService;
        private readonly ILogger<EmailController> _logger;

        public EmailController(IBackgroundJobClient jobClient, IEmailService emailService, IEmailReminderService emailReminderService, ILogger<EmailController> logger)
        {
            _jobClient = jobClient;
            _emailService = emailService;
            _emailReminderService = emailReminderService;
            _logger = logger;
        }

        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest emailRequest)
        {
            if (emailRequest == null || emailRequest.Recipients == null || emailRequest.Recipients.Length == 0
                || string.IsNullOrEmpty(emailRequest.Subject) || string.IsNullOrEmpty(emailRequest.Body))
            {
                _logger.LogError("Unexpected Error request not sent");
                return BadRequest("Invalid email request");
            }
            await _emailService.SendEmailAsync(emailRequest.Recipients,emailRequest.Subject, emailRequest.Body);
            return Ok("Email sent successfully");
        }

        [HttpPost("email-reminder")]
        public async Task<IActionResult> SendEmailReminder()
        {
            _jobClient.Enqueue(() => _emailReminderService.SendWeeklyRemindersAsync());
            return Ok("Reminder job has been triggered");
        }
    }
}
