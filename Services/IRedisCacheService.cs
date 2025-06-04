namespace QuizAppService.Services
{
    public interface IRedisCacheService
    {
        Task<T?> GetCachedValueAsync<T>(string key);
        Task SetCachedValueAsync<T>(string key, T value, TimeSpan? expiry = null);
        Task<bool> IsKeyCachedAsync(string key);
        Task ClearCachedValueAsync(string key);
    }
}
