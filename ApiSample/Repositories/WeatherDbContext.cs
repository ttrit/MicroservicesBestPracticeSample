using ApiSample.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ApiSample.Repositories
{
    public class WeatherDbContext : DbContext
    {
        private readonly IOptions<DatabaseSettings> _databaseOptions;

        public WeatherDbContext(IOptions<DatabaseSettings> databaseOptions)
        {
            _databaseOptions = databaseOptions;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _databaseOptions.Value.ConnectionString;

            optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<Weather> Weathers { get; set; }
    }
}
