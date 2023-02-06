using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Context
{
    public class IdentityDbContext : IdentityDbContext<IdentityUser>
    {

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(Domain.Models.DbConnections.IdentityDB, b => b.MigrationsAssembly("API"));


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            string[] strRoles = { "Admin", "User", "software", "accounting" };

            IdentityRole[] roles = (from role in strRoles
                                    select
                                    new IdentityRole()
                                    {
                                        Name = role,
                                        NormalizedName = role.ToUpperInvariant(),
                                        ConcurrencyStamp = Guid.NewGuid().ToString()
                                    }).ToArray();

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
