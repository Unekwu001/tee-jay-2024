using Data.AppDbContext;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Core.TokenServices.FetchValidTokenServices
{
    public class FetchValidTokens : IFetchValidTokens
    {
        private readonly TeejayDbContext _context;
        private readonly ILogger<FetchValidTokens> _logger;

        public FetchValidTokens(TeejayDbContext context, ILogger<FetchValidTokens> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<TokenInfo>> FetchValidTokensAsync()
        {
            try
            {
                var tokens = await _context.Tokens
                .Where(t => !t.IsUsed) // Filter where IsUsed is false
                .Select(t => new TokenInfo
                {
                    Id = t.Id,
                    Value = t.Value
                })
                .ToListAsync();
                return tokens;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
    }
}
