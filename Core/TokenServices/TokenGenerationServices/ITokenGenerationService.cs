using Data.Models;

namespace Core.TokenServices.TokenGenerationService
{
    public interface ITokenGenerationService
    {
        Task<IEnumerable<Token>> GenerateTokens(int numberOfTokens);
    }
}