
namespace Core.AttendantUserServices.LinkAttendantToTokenServices
{
    public interface ILinkAttendantToTokenService
    {
        Task<bool> LinkAttendantToToken(string attendantId, string tokenId);
    }
}