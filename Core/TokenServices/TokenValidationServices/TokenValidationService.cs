using Data.AppDbContext;


namespace Core.TokenServices.TokenValidationService
{
    public class TokenValidationService : ITokenValidationService
    {
        private readonly TeejayDbContext _context;

        public TokenValidationService(TeejayDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ValidateToken(string tokenValue)
        {
            var token = _context.Tokens.SingleOrDefault(t => t.Value == tokenValue && !t.IsUsed);
            if (token != null)
            {
                token.IsUsed = true;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
