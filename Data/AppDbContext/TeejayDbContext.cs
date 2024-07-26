using Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.AppDbContext
{
    public class TeejayDbContext : IdentityDbContext<AdminUser>
    {
        public TeejayDbContext(DbContextOptions<TeejayDbContext> options) : base(options)
        {
        }
        

        public DbSet<Attendant> Attendants { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
        }

    }
}
