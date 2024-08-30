using Core.TokenServices.FetchValidTokenServices;
using Core.TokenServices.TokenGenerationService;
using Core.TokenServices.TokenValidationService;
using Core.TokenServices.UseTokenServices;
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
        private readonly IUseTokenService _useTokenService;
        private readonly ILogger<TokenController> _logger;

        public TokenController(ITokenGenerationService tokenGenerationService, ITokenValidationService tokenValidationService, IFetchValidTokens fetchValidTokens, IUseTokenService useTokenService, ILogger<TokenController> logger)
        {
            _tokenGenerationService = tokenGenerationService;
            _tokenValidationService = tokenValidationService;
            _fetchValidTokens = fetchValidTokens;
            _useTokenService = useTokenService;
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
                return Ok("Token is still valid");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"An error occured while validating token");
                return BadRequest( "An error occured while validating token");  
            }
        }
        [HttpGet("get-all-valid-tokens")]
        public async Task<IActionResult> GetAllValidTokens()
        {
            try
            {
                var allValidTokens = await _fetchValidTokens.FetchValidTokensAsync();
                if (allValidTokens == null)
                {
                    return BadRequest("Couldnt fetch all valid tokens");
                }
                return Ok(allValidTokens);

            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "couldnt fetch all valid tokens");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("use-token")]
        public async Task<IActionResult> UseToken(string tokenValue)
        {
            try
            {

                if (tokenValue == null)
                {
                    return BadRequest("Token is null");
                }
                var result = await _useTokenService.UseTokenAsync(tokenValue);
                if (!result)
                {
                    return BadRequest("Token is null, is used or is not linked to an attendant yet");
                }
                return Ok("You are Welcome! To TeeJay2024");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong when using token");
                return BadRequest("Something went wrong when using token");
            }

        }


    }
}
