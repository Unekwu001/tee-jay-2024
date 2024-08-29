using Data.AppDbContext;
using Data.Dtos;
using Data.Models;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Core.AttendantUserServices
{
    public class AttendantOnboardingService : IAttendantOnboardingService
    {
        private readonly TeejayDbContext _context;
        private readonly ILogger<AttendantOnboardingService> _logger;
        private readonly IMapper _mapper;

        public AttendantOnboardingService(TeejayDbContext context, ILogger<AttendantOnboardingService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> OnboardAttendantAsync(AttendantDto attendantDto)
        {
            if (await _context.Attendants.AnyAsync(a => a.Email == attendantDto.Email))
            {
                _logger.LogWarning("Attendant with email '{Email}' already exists.", attendantDto.Email);
                return false;
            }

            var attendant = _mapper.Map<Attendant>(attendantDto);
            _context.Attendants.Add(attendant);
            await _context.SaveChangesAsync();

            // Additional onboarding logic here (e.g., send welcome email)

            return true;
        }
    }
}
