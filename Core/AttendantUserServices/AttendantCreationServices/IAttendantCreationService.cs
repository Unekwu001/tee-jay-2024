using Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Core.AttendantUserServices.AttendantCreationServices
{
    public interface IAttendantCreationService
    {
        Task<IdentityResult> CreateAttendantUserAsync(Attendant attendant, string password);
    }
}