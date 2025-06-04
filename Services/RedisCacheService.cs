
using StackExchange.Redis;
using System.Text.Json;

namespace QuizAppService.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDatabase _db;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task<T?> GetCachedValueAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);
            return value.HasValue ? JsonSerializer.Deserialize<T>(value) : default;
        }

        public async Task SetCachedValueAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            expiry ??= TimeSpan.FromMinutes(30);

            var json = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, json, expiry);
        }
        public async Task<bool> IsKeyCachedAsync(string key)
        {
            return await _db.KeyExistsAsync(key);
        }

        public async Task ClearCachedValueAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }
    }
}
