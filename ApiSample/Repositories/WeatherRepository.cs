using MCR.App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace MCR.App.Repositories
{
    public interface IWeatherRepository
    {
        Task<Weather> GetById(Guid id, CancellationToken cancellationToken);
    }

    public class WeatherRepository : IWeatherRepository
    {
        private readonly IDistributedCache _distributedCache;
        private readonly WeatherDbContext _context;

        public WeatherRepository(IDistributedCache distributedCache, WeatherDbContext context)
        {
            _distributedCache = distributedCache;
            _context = context;
        }

        public async Task<Weather> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            string key = $"weather-{id}";

            string? cachedWeather = await _distributedCache.GetStringAsync(
                key,
                cancellationToken);

            Weather? weather;
            if (string.IsNullOrEmpty(cachedWeather))
            {
                weather = await _context.Weathers.FirstOrDefaultAsync(w => w.Id == id);

                if (weather is null)
                {
                    return weather;
                }

                await _distributedCache.SetStringAsync(
                    key, 
                    JsonSerializer.Serialize(weather), 
                    cancellationToken);

                return weather;
            }

            weather = JsonSerializer.Deserialize<Weather>(cachedWeather);

            return weather;
        }
    }
}
