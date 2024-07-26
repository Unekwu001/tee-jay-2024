using Data.Dtos;
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

        //public async Task<IdentityResult> CreateAttendantUserAsync(Attendant attendant, string password)
        //{
        //    var result = await _userManager.CreateAsync(attendant);
        //    if (!result.Succeeded)
        //    {
        //        var errors = string.Join(",", result.Errors.Select(e => e.Description));
        //        _logger.LogError($"Failed to create {attendant.Email}, Errors:{errors}");
        //    }
        //    return result;
        //}

        public async Task<IdentityResult> CreateAttendantAsync(AttendantDto attendantDto)
        {
            var user = new Attendant
            {
                FirstName = attendantDto.FirstName,
                LastName = attendantDto.LastName,
                UserName = attendantDto.Email,
                Email = attendantDto.Email,
                Family = attendantDto.Family,
                Relationship = attendantDto.Relationship,
                PhoneNumber = attendantDto.PhoneNumber,
                Url = attendantDto.Url,
            };

            var result = await _userManager.CreateAsync(user, attendantDto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(",", result.Errors.Select(e => e.Description));
                _logger.LogError($"Failed to create {attendantDto.Email}, Errors:{errors}");
            }
            return result;

        }

    }
}



