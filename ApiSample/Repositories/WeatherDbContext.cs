using MCR.App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MCR.App.Repositories
{
    public class WeatherDbContext : DbContext
    {
        public WeatherDbContext(DbContextOptions<WeatherDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        public DbSet<Weather> Weathers { get; set; }
    }
}
