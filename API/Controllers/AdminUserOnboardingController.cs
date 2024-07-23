using Core.AdminUserServices;
using Data.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminUserController : ControllerBase
    {
        private readonly IAdminUserService _adminUserService;
        private readonly ILogger<AdminUserController> _logger;

        public AdminUserController(IAdminUserService adminUserService, ILogger<AdminUserController> logger)
        {
            _adminUserService = adminUserService;
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAdminUser([FromBody] AdminUserDto adminUserDto)
        {
            if (adminUserDto == null)
            {
                _logger.LogWarning("AdminUserDto is null.");
                return BadRequest("Admin user data cannot be null.");
            }

            try
            {
                var result = await _adminUserService.CreateAdminAsync(adminUserDto);

                if (result.Succeeded)
                {
                    return Ok("Admin user created successfully.");
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("Failed to create admin user. Errors: {Errors}", errors);
                    return BadRequest(errors);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the admin user.");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
    }
}

