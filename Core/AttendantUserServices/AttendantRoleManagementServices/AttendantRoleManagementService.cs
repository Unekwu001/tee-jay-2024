using AutoMapper;
using Data.Dtos;
using Data.Enums;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.AdminUserServices.RoleManagementServices.AdminRoleManagementService;

namespace Core.AttendatUserServices.RoleManagementServices
{
    public class AttendantRoleManagementService : IAttendantRoleManagementService
    {
        private readonly UserManager<Attendant> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AttendantRoleManagementService> _logger;
        private readonly IMapper _mapper;

        public AttendantRoleManagementService(
            UserManager<Attendant> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<AttendantRoleManagementService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<IdentityResult> AssignRoleAsync(AttendantDto attendantDto)
        {
            // Retrieve roles from UserRole enum
            string[] familyRoles = Enum.GetNames(typeof(FamilyMember));
            string[] relationshipRoles = Enum.GetNames(typeof(RelationshipType));

            // Create roles if they do not exist
            IdentityResult familyRoleResult = IdentityResult.Success;
            IdentityResult relationshipRoleResult = IdentityResult.Success;

            foreach (var role in familyRoles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    familyRoleResult = await _roleManager.CreateAsync(new IdentityRole(role));
                    if (!familyRoleResult.Succeeded)
                    {
                        var roleErrors = string.Join(", ", familyRoleResult.Errors.Select(e => e.Description));
                        _logger.LogWarning("Failed to create role '{Role}'. Errors: {Errors}", role, roleErrors);
                        return IdentityResult.Failed(familyRoleResult.Errors.ToArray());
                    }
                }
            }

            foreach (var role in relationshipRoles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    relationshipRoleResult = await _roleManager.CreateAsync(new IdentityRole(role));
                    if (!relationshipRoleResult.Succeeded)
                    {
                        var roleErrors = string.Join(", ", relationshipRoleResult.Errors.Select(e => e.Description));
                        _logger.LogWarning("Failed to create role '{Role}'. Errors: {Errors}", role, roleErrors);
                        return IdentityResult.Failed(relationshipRoleResult.Errors.ToArray());
                    }
                }
            }
            // Map AdminUserDto to AdminUser
            var attendant = _mapper.Map<Attendant>(attendantDto);

            // Assign the role to the user
            var addToFamilyRoleResult = await _userManager.AddToRoleAsync(attendant, attendantDto.Family.ToString());
            if (!addToFamilyRoleResult.Succeeded)
            {
                var roleErrors = string.Join(", ", addToFamilyRoleResult.Errors.Select(e => e.Description));
                _logger.LogWarning("Failed to assign role '{Role}' to user '{Email}'. Errors: {Errors}", attendantDto.Family, attendantDto.Email, roleErrors);
                return IdentityResult.Failed(addToFamilyRoleResult.Errors.ToArray());
            }

            var addToRelationshipRoleResult = await _userManager.AddToRoleAsync(attendant, attendantDto.Family.ToString());
            if (!addToRelationshipRoleResult.Succeeded)
            {
                var roleErrors = string.Join(", ", addToRelationshipRoleResult.Errors.Select(e => e.Description));
                _logger.LogWarning("Failed to assign role '{Role}' to user '{Email}'. Errors: {Errors}", attendantDto.Relationship, attendantDto.Email, roleErrors);
                return IdentityResult.Failed(addToRelationshipRoleResult.Errors.ToArray());
            }

            return IdentityResult.Success;
        }

    }
}


// Retrieve the user from the database (you may need to adjust this based on your actual implementation)
//var adminUser = await _userManager.FindByEmailAsync(adminUserDto.Email);
//if (adminUser == null)
//{
//    _logger.LogWarning("User with email '{Email}' not found.", adminUserDto.Email);
//    return IdentityResult.Failed(new IdentityError { Description = "User not found" });
//}