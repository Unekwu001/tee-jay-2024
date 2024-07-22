using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.AdminUserServices.RoleManagementServices.RoleManagementService;

namespace Core.AdminUserServices.RoleManagementServices
{
    public class RoleManagementService : IRoleManagementService
    {
        private readonly UserManager<AdminUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RoleManagementService> _logger;

        public RoleManagementService(
            UserManager<AdminUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RoleManagementService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<IdentityResult> AssignRoleAsync(AdminUser adminUser)
        {
            if (!await _roleManager.RoleExistsAsync(adminUser.Role.ToString()))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(adminUser.Role.ToString()));
                if (!roleResult.Succeeded)
                {
                    var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    _logger.LogWarning("Failed to create role '{Role}'. Errors: {Errors}", adminUser.Role, roleErrors);
                    return IdentityResult.Failed(roleResult.Errors.ToArray());
                }
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(adminUser, adminUser.Role.ToString());
            if (!addToRoleResult.Succeeded)
            {
                var roleErrors = string.Join(", ", addToRoleResult.Errors.Select(e => e.Description));
                _logger.LogWarning("Failed to assign role '{Role}' to user '{Email}'. Errors: {Errors}", adminUser.Role, adminUser.Email, roleErrors);
                return IdentityResult.Failed(addToRoleResult.Errors.ToArray());
            }

            return IdentityResult.Success;
        }
    } 
}
