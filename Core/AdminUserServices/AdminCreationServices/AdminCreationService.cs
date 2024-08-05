using Data.Dtos;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AdminUserServices.AdminCreationServices
{
    public class AdminCreationService : IAdminCreationService
    {
        private readonly UserManager<AdminUser> _userManager;
        private readonly ILogger<AdminCreationService> _logger;

        public AdminCreationService(UserManager<AdminUser> userManager, ILogger<AdminCreationService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IdentityResult> CreateUserAsync(AdminUserDto adminUserDto)
        {
            var user = new AdminUser
            {
                FullName = adminUserDto.FullName,
                UserName = adminUserDto.Email,
                Email = adminUserDto.Email,
            };

            var result = await _userManager.CreateAsync(user, adminUserDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Failed to create user '{Email}'. Errors: {Errors}", adminUserDto.Email, errors);       
            }

            return result;
        }
    }
}
