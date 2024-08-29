using Data.Dtos;

namespace Core.AttendantUserServices
{
    public interface IAttendantOnboardingService
    {
        Task<bool> OnboardAttendantAsync(AttendantDto attendantDto);
    }
}