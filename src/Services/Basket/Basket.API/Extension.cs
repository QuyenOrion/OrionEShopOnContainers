using StackExchange.Redis;

namespace Basket.API
{
    public static class Extension
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(sp =>
            {
                var redisConfiguration = ConfigurationOptions.Parse(configuration.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(redisConfiguration);
            });

            return services;
        }
    }
}
