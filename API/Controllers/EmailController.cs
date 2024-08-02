using Core.EmailServices;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("/api[controler]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest emailRequest)
        {
            if (emailRequest == null || emailRequest.Recipients == null || emailRequest.Recipients.Length == 0
                || string.IsNullOrEmpty(emailRequest.Subject) || string.IsNullOrEmpty(emailRequest.Body))
            {
                return BadRequest("Invalid email request");
            }
            await _emailService.SendEmailAsync(emailRequest.Recipients,emailRequest.Subject, emailRequest.Body);
            return Ok("Email sent successfully");
        }
    }
}
