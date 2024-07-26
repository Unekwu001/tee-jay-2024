using Data.AppDbContext;
using Data.Dtos;
using Data.Enums;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AttendantController : ControllerBase
{
    private readonly TeejayDbContext _context;

    public AttendantController(TeejayDbContext context)
    {
        _context = context;
    }

    [HttpPost("onboard")]
    public async Task<IActionResult> OnboardAttendant([FromBody] AttendantDto attendantDto)
    {
        var attendant = new Attendant
        {
            Id = Guid.NewGuid(),
            FirstName = attendantDto.FirstName,
            LastName = attendantDto.LastName,
            Family = attendantDto.Family,
            Relationship = attendantDto.Relationship,
            PhoneNumber = attendantDto.PhoneNumber,
            Email = attendantDto.Email,
            Url = attendantDto.Url
        };

        _context.Attendants.Add(attendant);
        await _context.SaveChangesAsync();

        return Ok("Attendant onboarded successfully.");
    }
}

//public class AttendantDto
//{
//    public string FirstName { get; set; }
//    public string LastName { get; set; }
//    public FamilyMember Family { get; set; }
//    public RelationshipType Relationship { get; set; }
//    public string PhoneNumber { get; set; }
//    public string Email { get; set; }
//    public string Url { get; set; }
//}
