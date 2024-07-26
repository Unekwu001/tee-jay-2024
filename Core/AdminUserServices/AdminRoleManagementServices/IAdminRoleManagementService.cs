using Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.AdminUserServices.RoleManagementServices
{
    public interface IAdminRoleManagementService
    {
        Task<IdentityResult> AssignRoleAsync(AdminUser adminUser);
    }
}
