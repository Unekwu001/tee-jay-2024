using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly string _logFilePath = "logs/log-.txt";

        [HttpGet("read")]
        public async Task<IActionResult> ReadLogs()
        {
            if (!System.IO.File.Exists(_logFilePath))
            {
                return NotFound("Log file not found.");
            }

            var logs = await System.IO.File.ReadAllTextAsync(_logFilePath);
            return Ok(logs);
        }
    }
}

