using Data.AppDbContext;
using Microsoft.EntityFrameworkCore;


namespace Core.TokenServices.UseTokenServices
{
    public class UseTokenService : IUseTokenService
    {
        private readonly TeejayDbContext _context;

        public UseTokenService(TeejayDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UseTokenAsync(string tokenValue)
        {
            var token = await _context.Tokens
                .FirstOrDefaultAsync(t => t.Value == tokenValue);

            if (token == null || token.IsUsed)
            {
                return false;
            }

            var attendant = await _context.Attendants
                .FirstOrDefaultAsync(a => a.TokenId == token.Id.ToString());

            if (attendant == null ||  !attendant.TokenId.Equals(token.Id.ToString(),StringComparison.OrdinalIgnoreCase) )
            {
                return false;
            }

            token.IsUsed = true;
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
