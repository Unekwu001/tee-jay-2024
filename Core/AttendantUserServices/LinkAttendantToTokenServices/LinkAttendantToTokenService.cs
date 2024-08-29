using Data.AppDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AttendantUserServices.LinkAttendantToTokenServices
{
    public class LinkAttendantToTokenService : ILinkAttendantToTokenService
    {
        private readonly TeejayDbContext _context;

        public LinkAttendantToTokenService(TeejayDbContext context)
        {
            _context = context;
        }

        public async Task<bool> LinkAttendantToToken(Guid attendantId, Guid tokenId)
        {
            var attendant = _context.Attendants.FirstOrDefault(a => a.Id == attendantId);
            if (attendant != null)
            {
                attendant.TokenId = tokenId.ToString();
                return true;
            }
            return false;

        }
    }
}
