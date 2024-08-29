using Data.Models;

namespace Core.AttendantUserServices
{
    public interface IFetchAttendantsNameAndIdService
    {
        Task<List<AttendantInfo>> FetchAllAttendantsNameAndIdAsync();
    }
}