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

        public AttendantCreationService(UserManager<Attendant> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> CreateAttendantAsync(AttendantDto attendantDto)
        {
            var user = new Attendant
            {
                FirstName = attendantDto.FirstName,
                LastName = attendantDto.LastName,
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
            }
            return result;

        }

    }
}



