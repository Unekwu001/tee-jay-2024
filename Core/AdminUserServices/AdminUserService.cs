using Core.AdminUserServices.AdminCreationServices;
using Core.AdminUserServices.RoleManagementServices;
using Data.Dtos;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Core.AdminUserServices
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IAdminCreationService _adminCreationService;
        private readonly IRoleManagementService _roleManagementService;
        private readonly ILogger<AdminUserService> _logger;

        public AdminUserService(IAdminCreationService userCreationService,IRoleManagementService roleManagementService,ILogger<AdminUserService> logger)
        {
            _adminCreationService = userCreationService;
            _roleManagementService = roleManagementService;
            _logger = logger;
        }

        public async Task<IdentityResult> CreateAdminAsync(AdminUserDto adminUserDto)
        {
            if (adminUserDto == null)
            {
                _logger.LogWarning("AdminUserDto is null.");
                throw new ArgumentNullException(nameof(adminUserDto), "Admin user data cannot be null.");
            }

            var adminUser = new AdminUser
            {
                FullName = adminUserDto.FullName,
                UserName = adminUserDto.Email,
                Email = adminUserDto.Email,
                Role = adminUserDto.Role
            };

            try
            {
                var result = await _adminCreationService.CreateAdminUserAsync(adminUser, adminUserDto.Password);

                if (result.Succeeded)
                {
                    var roleResult = await _roleManagementService.AssignRoleAsync(adminUser);

                    if (!roleResult.Succeeded)
                    {
                        return IdentityResult.Failed(roleResult.Errors.ToArray());
                    }

                    _logger.LogInformation("Admin user '{Email}' onboarded successfully with role '{Role}'.", adminUser.Email, adminUser.Role);
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("Failed to create admin user '{Email}'. Errors: {Errors}", adminUser.Email, errors);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while onboarding admin user '{Email}'.", adminUser.Email);
                throw; // Re-throw the exception to ensure it is handled by the caller if necessary
            }
        }
    }
}
