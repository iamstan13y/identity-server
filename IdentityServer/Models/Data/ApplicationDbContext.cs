using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Models.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Account>? Accounts { get; set; }
    }
}
