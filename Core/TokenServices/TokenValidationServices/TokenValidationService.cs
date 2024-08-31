using Data.AppDbContext;
using Microsoft.Extensions.Logging;


namespace Core.TokenServices.TokenValidationService
{
    public class TokenValidationService : ITokenValidationService
    {
        private readonly TeejayDbContext _context;
        private readonly ILogger<ITokenValidationService> _logger;
        public TokenValidationService(TeejayDbContext context, ILogger<ITokenValidationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> ValidateToken(string tokenValue)
        {
            try
            {
                var token = _context.Tokens.SingleOrDefault(t => t.Value == tokenValue && !t.IsUsed);
                if (token == null)
                {
                    return false;
                }
                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
    }
}
