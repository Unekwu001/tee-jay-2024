using API;
using Core.AdminUserServices;
using Core.AdminUserServices.AdminCreationServices;
using Core.AdminUserServices.RoleManagementServices;
using Core.AttendantUserServices;
using Core.AttendantUserServices.AttendantCreationServices;
using Core.AttendantUserServices.AttendantRoleManagementServices;
using Data.AppDbContext;
using Data.Enums;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace TeeJay
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure Serilog
            builder.Host.UseSerilog((context, LoggerConfig) => LoggerConfig.ReadFrom.Configuration(context.Configuration));

            // Register services
            builder.Services.AddScoped<IAdminCreationService, AdminCreationService>();
            builder.Services.AddScoped<IAdminRoleManagementService, AdminRoleManagementService>();
            //builder.Services.AddScoped<IAttendantCreationService, AttendantCreationService>();
            builder.Services.AddScoped<IAttendantOnboardingService, AttendantOnboardingService>();

            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Get connectionString
            var connectionString = builder.Configuration.GetConnectionString("TeeJayConnection");
            builder.Services.AddDbContext<TeejayDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Configure Identity for AdminUser and Attendant
            builder.Services.AddIdentity<AdminUser, IdentityRole>(options => options.User.RequireUniqueEmail = true)
                .AddEntityFrameworkStores<TeejayDbContext>()
                .AddDefaultTokenProviders();



            //Use same userManager and roleManager for Attendant
            builder.Services.AddScoped<UserManager<AdminUser>>();
            builder.Services.AddScoped<RoleManager<IdentityRole>>();

            // Configure Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
