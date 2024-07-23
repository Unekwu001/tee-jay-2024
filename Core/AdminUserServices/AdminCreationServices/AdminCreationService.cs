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

        public async Task<IdentityResult> CreateAdminUserAsync(AdminUser adminUser, string password)
        {
            var result = await _userManager.CreateAsync(adminUser, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Failed to create user '{Email}'. Errors: {Errors}", adminUser.Email, errors);
            }

            return result;
        }
    }
}
