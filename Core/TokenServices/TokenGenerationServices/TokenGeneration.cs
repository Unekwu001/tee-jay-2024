using Data.AppDbContext;
using Data.Models;


namespace Core.TokenServices.TokenGenerationService
{
    public class TokenGenerationService : ITokenGenerationService
    {
        private readonly TeejayDbContext _context;
        private static readonly Random _random = new Random();

        public TokenGenerationService(TeejayDbContext context)
        {
            _context = context;
        }

        public List<Token> GenerateTokens(int numberOfTokens)
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
            _context.SaveChanges();
            return tokens;
        }

        private string GenerateRandomToken()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string token;

            do
            {
                token = new string(Enumerable.Repeat(chars, 8)
                    .Select(s => s[_random.Next(s.Length)]).ToArray());
            } while (!token.Any(char.IsLetter) || !token.Any(char.IsDigit));

            return token;
        }



    }
}
