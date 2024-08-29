using Core.TokenServices.FetchValidTokenServices;
using Core.TokenServices.TokenGenerationService;
using Core.TokenServices.TokenValidationService;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController: ControllerBase
    {
        private readonly ITokenGenerationService _tokenGenerationService;
        private readonly ITokenValidationService _tokenValidationService;
        private readonly IFetchValidTokens _fetchValidTokens;
        private readonly ILogger<TokenController> _logger;

        public TokenController(ITokenGenerationService tokenGenerationService, ITokenValidationService tokenValidationService, ILogger<TokenController> logger, IFetchValidTokens fetchValidTokens)
        {
            _tokenGenerationService = tokenGenerationService;
            _tokenValidationService = tokenValidationService;
            _fetchValidTokens = fetchValidTokens;
            _logger = logger;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateTokens([FromBody] int numberOfTokens)
        {
            try
            {
                if (numberOfTokens <= 0 || numberOfTokens > 100) 
                {
                    return BadRequest("Number of tokens should be betwenn 1 and 100");
                }
                var tokens = await _tokenGenerationService.GenerateTokens(numberOfTokens);
                return Ok(tokens);
            }
            catch (ArgumentException ex) 
            {
                _logger.LogWarning(ex, "invalid number of tokens requested");
                return BadRequest(ex.Message);  
            }
            catch(Exception ex)
            {

                _logger.LogError(ex, "An error occurred while generating tokens.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateToken([FromBody] string token)
        {
            try
            {
                var isValid = await _tokenValidationService.ValidateToken(token);
                if (!isValid)
                {
                    return BadRequest("Invalid token");
                }
                return Ok(isValid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"An error occured while validating token");
                return BadRequest( "An error occured while validating token");  
            }
        }
        [HttpGet("get-all-valid-tokens")]
        public async Task<IActionResult> GetAllValidTokensAsync()
        {
            var allValidTokens = await _fetchValidTokens.FetchValidTokensAsync();
            return Ok(allValidTokens);
        }


    }
}
