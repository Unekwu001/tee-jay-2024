using Data.AppDbContext;


namespace Core.AttendantUserServices.LinkAttendantToTokenServices
{
    public class LinkAttendantToTokenService : ILinkAttendantToTokenService
    {
        private readonly TeejayDbContext _context;

        public LinkAttendantToTokenService(TeejayDbContext context)
        {
            _context = context;
        }

        public async Task<bool> LinkAttendantToToken(string attendantId, string tokenId)
        {
            var attendant = _context.Attendants.FirstOrDefault(a => a.Id.ToString() == attendantId);
            var token = _context.Tokens.FirstOrDefault(t => t.Id.ToString() == tokenId);

            if (attendant != null && !token.IsUsed)
            {
                
                attendant.TokenId = tokenId.ToString();
                await _context.SaveChangesAsync();
                return true;
            }
            
            return false;

        }
    }
}
