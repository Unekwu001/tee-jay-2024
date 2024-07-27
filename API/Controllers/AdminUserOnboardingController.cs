using Core.AdminUserServices.AdminCreationServices;
using Core.AdminUserServices.RoleManagementServices;
using Data.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminCreationService _adminCreationService;
        private readonly IAdminRoleManagementService _adminRoleManagementService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            IAdminCreationService adminCreationService,
            IAdminRoleManagementService adminRoleManagementService,
            ILogger<AdminController> logger)
        {
            _adminCreationService = adminCreationService;
            _adminRoleManagementService = adminRoleManagementService;
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAdmin([FromBody] AdminUserDto adminUserDto)
        {
            var result = await _adminCreationService.CreateUserAsync(adminUserDto);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Admin creation failed for '{Email}'.", adminUserDto.Email);
                return BadRequest(result.Errors);
            }

            var roleResult = await _adminRoleManagementService.AssignRoleAsync(adminUserDto);
            if (!roleResult.Succeeded)
            {
                _logger.LogWarning("Role assignment failed for admin '{Email}'.", adminUserDto.Email);
                return BadRequest(roleResult.Errors);
            }

            return Ok("Admin user created and role assigned successfully.");
        }
    }
}

