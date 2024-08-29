using Data.AppDbContext;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AttendantUserServices
{
    public class FetchAttendantsNameAndIdService : IFetchAttendantsNameAndIdService
    {
        private readonly TeejayDbContext _context;
        public FetchAttendantsNameAndIdService(TeejayDbContext context)
        {
            _context = context;
        }

        public async Task<List<AttendantInfo>> FetchAllAttendantsNameAndIdAsync()
        {
            var attendantNameAndId = await _context.Attendants.Select(
                a => new AttendantInfo
                {
                    Id = a.Id,
                    FullName = a.FirstName + " " + a.LastName
                }).ToListAsync();

            return attendantNameAndId;
        }



    }
}
