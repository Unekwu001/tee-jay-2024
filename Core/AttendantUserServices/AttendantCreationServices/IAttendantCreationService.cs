using Data.Dtos;
using Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Core.AttendantUserServices.AttendantCreationServices
{
    public interface IAttendantCreationService
    {
        Task<IdentityResult> CreateAttendantAsync(AttendantDto attendantDto);
    }
}