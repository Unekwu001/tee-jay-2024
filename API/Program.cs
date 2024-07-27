
using Core.AdminUserServices;
using Core.AdminUserServices.AdminCreationServices;
using Core.AdminUserServices.RoleManagementServices;
using Data.AppDbContext;
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
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Host.UseSerilog((context, LoggerConfig) => LoggerConfig.ReadFrom.Configuration(context.Configuration));
            builder.Services.AddScoped<IAdminCreationService, AdminCreationService>();
            builder.Services.AddScoped<IAdminUserService, AdminUserService>();
            builder.Services.AddScoped<IRoleManagementService, RoleManagementService>();
            builder.Services.AddControllers();
            builder.Services.AddIdentity<AdminUser, IdentityRole>().AddEntityFrameworkStores<TeejayDbContext>()
                            .AddDefaultTokenProviders();
            var connectionString = builder.Configuration.GetConnectionString("TeeJayConnection");
            object value = builder.Services.AddDbContext<TeejayDbContext>(options =>
                options.UseSqlServer(connectionString));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline//.
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
