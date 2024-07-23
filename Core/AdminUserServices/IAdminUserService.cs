using Data.Dtos;
using Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Core.AdminUserServices
{
    public interface IAdminUserService
    {
        Task<IdentityResult> CreateAdminAsync(AdminUserDto adminUserDto);    }
}