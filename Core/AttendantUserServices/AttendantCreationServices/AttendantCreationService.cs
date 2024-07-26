using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AttendantUserServices.AttendantCreationServices
{
    public class AttendantCreationService : IAttendantCreationService
    {
        private readonly UserManager<Attendant> _userManager;
        private readonly ILogger<AttendantCreationService> _logger;

        public AttendantCreationService(UserManager<Attendant> userManager, ILogger<AttendantCreationService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IdentityResult> CreateAttendantUserAsync(Attendant attendant, string password)
        {
            var result = await _userManager.CreateAsync(attendant);
            if (!result.Succeeded)
            {
                var errors = string.Join(",", result.Errors.Select(e => e.Description));
                _logger.LogError($"Failed to create {attendant.email}, Errors:{errors}");
            }
            return result;
        }
    }
}
