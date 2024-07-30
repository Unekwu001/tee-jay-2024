using Data.AppDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.TokenServices.TokenValidationService
{
    public class TokenValidationService : ITokenValidationService
    {
        private readonly TeejayDbContext _context;

        public TokenValidationService(TeejayDbContext context)
        {
            _context = context;
        }

        public bool ValidateToken(string tokenValue)
        {
            var token = _context.Tokens.SingleOrDefault(t => t.Value == tokenValue && !t.IsUsed);
            if (token != null)
            {
                token.IsUsed = true;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
