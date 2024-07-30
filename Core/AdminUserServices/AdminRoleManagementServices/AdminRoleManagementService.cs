using AutoMapper;
using Data.Dtos;
using Data.Enums;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Core.AdminUserServices.RoleManagementServices
{
    public class AdminRoleManagementService : IAdminRoleManagementService
    {
        private readonly UserManager<AdminUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AdminRoleManagementService> _logger;
        private readonly IMapper _mapper;

        public AdminRoleManagementService(UserManager<AdminUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<AdminRoleManagementService> logger, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IdentityResult> AssignRoleAsync(AdminUserDto adminUserDto)
        {
            // Retrieve roles from UserRole enum
            string[] roles = Enum.GetNames(typeof(UserRole));

            // Create roles if they do not exist
            IdentityResult roleResult = IdentityResult.Success;
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    roleResult = await _roleManager.CreateAsync(new IdentityRole(role));
                    if (!roleResult.Succeeded)
                    {
                        var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                        _logger.LogWarning("Failed to create role '{Role}'. Errors: {Errors}", role, roleErrors);
                        return IdentityResult.Failed(roleResult.Errors.ToArray());
                    }
                }
            }

            // Retrieve the user from the database
            var adminUser = await _userManager.FindByEmailAsync(adminUserDto.Email);
            if (adminUser == null)
            {
                // Map AdminUserDto to AdminUser
                adminUser = _mapper.Map<AdminUser>(adminUserDto);
                var createUserResult = await _userManager.CreateAsync(adminUser, adminUserDto.Password);
                if (!createUserResult.Succeeded)
                {
                    var createUserErrors = string.Join(", ", createUserResult.Errors.Select(e => e.Description));
                    _logger.LogWarning("Failed to create user '{Email}'. Errors: {Errors}", adminUserDto.Email, createUserErrors);
                    return IdentityResult.Failed(createUserResult.Errors.ToArray());
                }
            }

            // Assign the role to the user
            var addToRoleResult = await _userManager.AddToRoleAsync(adminUser, adminUserDto.Role.ToString());
            if (!addToRoleResult.Succeeded)
            {
                var roleErrors = string.Join(", ", addToRoleResult.Errors.Select(e => e.Description));
                _logger.LogWarning("Failed to assign role '{Role}' to user '{Email}'. Errors: {Errors}", adminUserDto.Role, adminUserDto.Email, roleErrors);
                return IdentityResult.Failed(addToRoleResult.Errors.ToArray());
            }

            return IdentityResult.Success;
        }
    }
}
