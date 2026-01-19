using FinancePersonalTracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace FinancePersonalTracker.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<FamilyGroup> FamilyGroups { get; set; }
        public DbSet<FamilyGroupInvite> FamilyGroupInvites { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.UseSnakeCaseNamingConvention();

            base.OnModelCreating(builder);
        }
    }
}
