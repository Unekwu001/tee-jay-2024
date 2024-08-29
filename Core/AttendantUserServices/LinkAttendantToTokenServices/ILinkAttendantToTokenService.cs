
namespace Core.AttendantUserServices.LinkAttendantToTokenServices
{
    public interface ILinkAttendantToTokenService
    {
        Task<bool> LinkAttendantToToken(Guid attendantId, Guid tokenId);
    }
}