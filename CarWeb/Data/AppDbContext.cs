using CarWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace CarWeb.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Car> Cars { get; set; }
    }
}
