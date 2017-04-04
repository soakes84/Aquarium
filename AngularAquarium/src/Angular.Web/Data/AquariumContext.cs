using Angular.Web.Models;
using Aquarium.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aquarium.Data
{
    public class AquariumContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Fish> Fishes { get; set; }

        public DbSet<Tank> Tanks { get; set; }

        public AquariumContext() : base()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=Aquarium.db");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .ToTable("Users");
            builder.Entity<IdentityRole>()
                .ToTable("Roles");
            builder.Entity<IdentityRoleClaim<string>>()
                .ToTable("RoleClaims");
            builder.Entity<IdentityUserClaim<string>>()
                .ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>()
                .ToTable("UserLogins");
            builder.Entity<IdentityUserRole<string>>()
                .ToTable("UserRoles");
            builder.Entity<IdentityUserToken<string>>()
                .ToTable("UserTokens");
        }

    }
}
