using Data.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Core.AttendantUserServices.AttendantRoleManagementServices
{
    public interface IAttendantRoleManagementService
    {
        Task<IdentityResult> AssignRoleAsync(AttendantDto attendantDto);
    }
}