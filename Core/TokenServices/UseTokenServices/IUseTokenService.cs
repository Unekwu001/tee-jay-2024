
namespace Core.TokenServices.UseTokenServices
{
    public interface IUseTokenService
    {
        Task<bool> UseTokenAsync(string tokenValue);
    }
}