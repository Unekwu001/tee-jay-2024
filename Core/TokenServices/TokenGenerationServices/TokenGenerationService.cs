using System.Security.Cryptography;
using Data.AppDbContext;
using Data.Models;

namespace Core.TokenServices.TokenGenerationService
{
    public class TokenGenerationService : ITokenGenerationService
    {
        private readonly TeejayDbContext _context;

        public TokenGenerationService(TeejayDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Token>> GenerateTokens(int numberOfTokens)
        {
            if (numberOfTokens <= 0 || numberOfTokens > 100)
            {
                throw new ArgumentException("Number of tokens must be between 1 and 100.");
            }

            var tokens = new List<Token>();
            for (int i = 0; i < numberOfTokens; i++)
            {
                var tokenValue = GenerateRandomToken();
                var token = new Token { Value = tokenValue, IsUsed = false, CreatedAt = DateTime.UtcNow };
                tokens.Add(token);
                _context.Tokens.Add(token);
            }
            await _context.SaveChangesAsync();
            return tokens;
        }

        private string GenerateRandomToken()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var tokenBytes = new byte[8];
            var tokenChars = new char[8];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(tokenBytes);
            }

            for (int i = 0; i < tokenChars.Length; i++)
            {
                tokenChars[i] = chars[tokenBytes[i] % chars.Length];
            }

            string token = new string(tokenChars);

            // Ensure token contains at least one letter and one digit
            if (!token.Any(char.IsLetter) || !token.Any(char.IsDigit))
            {
                return GenerateRandomToken();
            }

            return token;
        }
    }
}
