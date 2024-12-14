using Microsoft.EntityFrameworkCore;

namespace SignalR.Covid19Chart.API.Models
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Covid> Covids { get; set; }
    }

}
