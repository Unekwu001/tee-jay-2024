using Data.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Core.AttendatUserServices.RoleManagementServices
{
    public interface IAttendantRoleManagementService
    {
        Task<IdentityResult> AssignRoleAsync(AttendantDto attendantDto);
    }
}