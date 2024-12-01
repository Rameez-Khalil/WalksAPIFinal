using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Walks.Api.Data
{
    public class WalksAuthDbContext : IdentityDbContext
    {
        public WalksAuthDbContext(DbContextOptions<WalksAuthDbContext> options) : base(options)
        {
        }

        //seed roles.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //create new Identity roles.
            var readerId = "9f86f2cc-8bef-4944-93ff-ddcad6df0849";
            var writerId = "9f86f2cc-8bef-4944-93ff-ddcad6df0846";

            //Create a list of user rols.
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = readerId,
                    Name = "Reader",
                    NormalizedName  = "Reader".ToUpper(),
                    ConcurrencyStamp = readerId
                },

                new IdentityRole {
                    Id = writerId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerId }
            }; 


           modelBuilder.Entity<IdentityRole>().HasData(roles);

        }
    }
}
