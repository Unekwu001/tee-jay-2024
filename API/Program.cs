
using API;
using Core.AdminUserServices;
using Core.AdminUserServices.AdminCreationServices;
using Core.AdminUserServices.RoleManagementServices;
using Core.AttendantUserServices.AttendantCreationServices;
using Core.AttendatUserServices.RoleManagementServices;
using Data.AppDbContext;
using Data.Enums;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Configuration;
using Serilog;
using System;

namespace TeeJay
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Host.UseSerilog((context, LoggerConfig) => LoggerConfig.ReadFrom.Configuration(context.Configuration));
            builder.Services.AddScoped<IAdminCreationService, AdminCreationService>();
            builder.Services.AddScoped<IAdminRoleManagementService, AdminRoleManagementService>();
            builder.Services.AddScoped<IAttendantCreationService, AttendantCreationService>();
            builder.Services.AddScoped<IAttendantRoleManagementService, AttendantRoleManagementService>();

            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddIdentity<AdminUser, IdentityRole>().AddEntityFrameworkStores<TeejayDbContext>()
                            .AddDefaultTokenProviders();
            var connectionString = builder.Configuration.GetConnectionString("TeeJayConnection");
            object value = builder.Services.AddDbContext<TeejayDbContext>(options =>
                options.UseSqlServer(connectionString));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            // Seed roles
            await CreateRoles(app.Services);

            app.Run();

            static async Task CreateRoles(IServiceProvider serviceProvider)
            {
                using var scope = serviceProvider.CreateScope();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                //string[] roles = { "SuperAdmin", "Regular", "Admin" };
                string[] roles = Enum.GetNames(typeof(UserRole));
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                string[] relationshipRoles = Enum.GetNames(typeof(RelationshipType));
                string[] familyRoles = Enum.GetNames(typeof(FamilyMember));

                foreach (var roleName in relationshipRoles)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                foreach (var roleName in familyRoles)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }
            }
        }
    }
}
