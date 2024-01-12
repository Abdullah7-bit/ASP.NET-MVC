using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace AspNetCoreCookieAuthentication.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
    }
}
