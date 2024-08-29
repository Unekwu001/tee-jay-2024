using Data.AppDbContext;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.TokenServices.FetchValidTokenServices
{
    public class FetchValidTokens : IFetchValidTokens
    {
        private readonly TeejayDbContext _context;

        public FetchValidTokens(TeejayDbContext context)
        {
            _context = context;
        }

        public async Task<List<TokenInfo>> FetchValidTokensAsync()
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
    }
}
