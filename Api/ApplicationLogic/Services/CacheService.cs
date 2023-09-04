using Api.ApplicationLogic.Interface;
using Api.Core;
using StackExchange.Redis;
using System.Text.Json;

#nullable disable

namespace Api.ApplicationLogic.Services
{
    public class CacheService : ICacheService
    {
        IDatabase _cacheDb;
        public CacheService(AppConfiguration configuration)
        {
            if (configuration.UseRedisCache)
            {
                var redis = ConnectionMultiplexer.Connect(configuration.ConnectionStrings.RedisConnectionDocker);
                _cacheDb = redis.GetDatabase();
            }
        }

        public async Task<T> Get<T>(string key)
        {
            var value = await _cacheDb.StringGetAsync(key);
            return value.HasValue ? JsonSerializer.Deserialize<T>(value) : default;
        }

        public async Task<bool> Remove(string key)
            => await _cacheDb.KeyDeleteAsync(key);

        public async Task<bool> Set<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var expiryTime = expirationTime - DateTimeOffset.Now;
            return await _cacheDb.StringSetAsync(key, JsonSerializer.Serialize(value), expiryTime);
        }

    }

}