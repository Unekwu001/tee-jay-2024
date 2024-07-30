using Data.Models;

namespace Core.TokenServices.TokenGenerationService
{
    public interface ITokenService
    {
        List<Token> GenerateTokens(int numberOfTokens);
    }
}