using Data.Dtos;
using Data.Models;

namespace Core.AttendantUserServices
{
    public interface IAttendantOnboardingService
    {
        Task<bool> OnboardAttendantAsync(AttendantDto attendantDto);
    }
}