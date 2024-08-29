using Core.AttendantUserServices;
using Core.AttendantUserServices.LinkAttendantToTokenServices;
using Data.Dtos;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendantController : ControllerBase
    {
        private readonly IAttendantOnboardingService _attendantOnboardingService;
        private readonly IFetchAttendantsNameAndIdService _fetchAttendantsNameAndIdService;
        private readonly ILinkAttendantToTokenService _linkAttendantToTokenService; 
        private readonly ILogger<AttendantController> _logger;

        public AttendantController(IAttendantOnboardingService attendantOnboardingService, IFetchAttendantsNameAndIdService fetchAttendantsNameAndIdService, ILinkAttendantToTokenService linkAttendantToTokenService, ILogger<AttendantController> logger)
        {
            _attendantOnboardingService = attendantOnboardingService;
            _fetchAttendantsNameAndIdService = fetchAttendantsNameAndIdService;
            _linkAttendantToTokenService = linkAttendantToTokenService;
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

        [HttpGet]
        public async Task<IActionResult> GetAllAttendantsNameAndId()
        {
            var allAttendantsNameAndId = await _fetchAttendantsNameAndIdService.FetchAllAttendantsNameAndIdAsync();
            return Ok(allAttendantsNameAndId);
        }

        [HttpPost("link-attendant-to-token")]
        public async Task<IActionResult> LinkAttendantToToken(string attendantId, string tokenId)
        {
            if(attendantId == null || tokenId == null)
            {
                return BadRequest("AttendantId or TokenId not specified");
            }
            var result = await _linkAttendantToTokenService.LinkAttendantToToken(attendantId, tokenId);  
            if (!result)
            {
                return NotFound("Error linking token to attendant");
            }
            return Ok(result);
        }
    }
}
