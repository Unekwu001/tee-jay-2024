using Core.AdminUserServices.AdminCreationServices;
using Core.AdminUserServices.RoleManagementServices;
using Core.AttendantUserServices;
using Core.EmailServices;
using Core.TokenServices.FetchValidTokenServices;
using Core.TokenServices.TokenGenerationService;
using Core.TokenServices.TokenValidationService;
using Data.AppDbContext;
using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace API
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
            // Attendant Service
            builder.Services.AddScoped<IFetchAttendantsNameAndIdService, FetchAttendantsNameAndIdService>();
            builder.Services.AddScoped<IAttendantOnboardingService, AttendantOnboardingService>();
            // Token Services
            builder.Services.AddScoped<ITokenGenerationService, TokenGenerationService>();
            builder.Services.AddScoped<ITokenValidationService, TokenValidationService>();
            builder.Services.AddScoped<IFetchValidTokens, FetchValidTokens>();
            // Email Services
            builder.Services.AddScoped<IEmailService, EmailService>();
            // Add Controllers
            builder.Services.AddControllers();

            builder.Services.AddAutoMapper(typeof(MappingProfile));

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

            // Use custom exception handling middleware
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Seed Email Settings
            //EmailSettingsInitializer.SeedEmailSettings(app.Services);

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            await app.RunAsync();
        }
    }
}
