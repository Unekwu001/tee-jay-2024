using Core.AttendantUserServices;
using Data.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendantController : ControllerBase
    {
        private readonly IAttendantOnboardingService _attendantOnboardingService;
        private readonly ILogger<AttendantController> _logger;

        public AttendantController(IAttendantOnboardingService attendantOnboardingService, ILogger<AttendantController> logger)
        {
            _attendantOnboardingService = attendantOnboardingService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> OnboardAttendant([FromBody] AttendantDto attendantDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for AttendantDto: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Starting onboarding process for attendant with email: {Email}", attendantDto.Email);

            var result = await _attendantOnboardingService.OnboardAttendantAsync(attendantDto);

            if (!result)
            {
                _logger.LogWarning("Failed to onboard attendant. Email already exists: {Email}", attendantDto.Email);
                return Conflict("Email already exists.");
            }

            _logger.LogInformation("Successfully onboarded attendant with email: {Email}", attendantDto.Email);
            return Ok("Attendant onboarded successfully.");
        }
    }
}
