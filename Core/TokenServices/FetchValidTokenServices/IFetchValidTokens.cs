using Data.Models;

namespace Core.TokenServices.FetchValidTokenServices
{
    public interface IFetchValidTokens
    {
        Task<List<TokenInfo>> FetchValidTokensAsync();
    }
}