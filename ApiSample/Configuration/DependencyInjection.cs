using FluentValidation;
using MCR.App.Repositories;
using Microsoft.EntityFrameworkCore;
using Scrutor;
using System.Reflection;

namespace MCR.App.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCaching(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(redisOptions =>
            {
                string connection = configuration.GetConnectionString("Redis");

                redisOptions.Configuration = connection;
            });

            return services;
        }

        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .Scan(
                    selector => selector
                        .FromAssemblies(

                        )
                        .AddClasses(false)
                        .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                        .AsMatchingInterface()
                        .WithScopedLifetime());
            services.AddDbContext<WeatherDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Database")));


            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
