using Data.Dtos;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AdminUserServices.AdminCreationServices
{
    public interface IAdminCreationService
    {
        Task<IdentityResult> CreateUserAsync(AdminUserDto adminUserDto);
    }
}
