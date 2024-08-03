using Core.EmailServices;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("/api[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<EmailController> _logger;

        public EmailController(IEmailService emailService, ILogger<EmailController> logger)
        {
            _emailService = emailService;
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
    }
}
