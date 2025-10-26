using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.Db
{
    public class UserDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        DbSet<IdentityUser> Users { get; set; }
        public UserDbContext(DbContextOptions<UserDbContext> options): base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
