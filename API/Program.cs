using Core.AdminUserServices.AdminCreationServices;
using Core.AdminUserServices.RoleManagementServices;
using Core.AttendantUserServices;
using Core.AttendantUserServices.LinkAttendantToTokenServices;
using Core.EmailServices;
using Core.ReminderServices;
using Core.TokenServices.FetchValidTokenServices;
using Core.TokenServices.TokenGenerationService;
using Core.TokenServices.TokenValidationService;
using Core.TokenServices.UseTokenServices;
using Data.AppDbContext;
using Data.Models;
using Hangfire;
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
            builder.Services.AddScoped<ILinkAttendantToTokenService, LinkAttendantToTokenService>();
            // Token Services
            builder.Services.AddScoped<ITokenGenerationService, TokenGenerationService>();
            builder.Services.AddScoped<ITokenValidationService, TokenValidationService>();
            builder.Services.AddScoped<IFetchValidTokens, FetchValidTokens>();
            builder.Services.AddScoped<IUseTokenService, UseTokenService>();
            // Email Services
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IEmailReminderService, EmailReminderService>();

            // Add Controllers
            builder.Services.AddControllers();

            // Configure AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Configure Entity Framework and Identity
            var connectionString = builder.Configuration.GetConnectionString("TeeJayConnection");
            builder.Services.AddDbContext<TeejayDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddIdentity<AdminUser, IdentityRole>(options => options.User.RequireUniqueEmail = true)
                .AddEntityFrameworkStores<TeejayDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddScoped<UserManager<AdminUser>>();
            builder.Services.AddScoped<RoleManager<IdentityRole>>();

            // Configure Hangfire
            builder.Services.AddHangfire(config =>
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                      .UseSimpleAssemblyNameTypeSerializer()
                      .UseRecommendedSerializerSettings()
                      .UseSqlServerStorage(connectionString)); // Use the same DB connection

            builder.Services.AddHangfireServer();

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

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            // Use Hangfire Dashboard (optional)
            app.UseHangfireDashboard();

            // Schedule the recurring job
            using (var scope = app.Services.CreateScope())
            {
                var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
                var reminderService = scope.ServiceProvider.GetRequiredService<IEmailReminderService>();

                recurringJobManager.AddOrUpdate(
                    "SendWeeklyReminders",
                    () => reminderService.SendWeeklyRemindersAsync(),
                    Cron.Weekly); // Every Saturday at midnight
            }

            await app.RunAsync();
        }
    }
}
