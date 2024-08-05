using Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;


namespace Data.AppDbContext
{
    public class TeejayDbContext : IdentityDbContext<AdminUser>
    {
        public TeejayDbContext(DbContextOptions<TeejayDbContext> options) : base(options)
        {
        }
        public DbSet<Attendant> Attendants { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<EmailSettings> EmailSettings { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Additional configurations for Attendants
           
            builder.Entity<Attendant>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).HasMaxLength(15);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Url).HasMaxLength(200);
            });
            
         





        }

    }
}
