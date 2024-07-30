using Data.Models;

namespace Core.TokenServices.TokenGenerationService
{
    public interface ITokenGenerationService
    {
        List<Token> GenerateTokens(int numberOfTokens);
    }
}