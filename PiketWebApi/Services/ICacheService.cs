using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace PiketWebApi.Services;


public interface ICacheService
{
    Task<T?> GetAsync<T>(string key, CancellationToken token = default) where T : class;
    Task<T?> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken token = default) where T : class;


    Task SetAsync<T>(string key, T value, CancellationToken token = default) where T : class;
    Task RemoveAsync(string key, CancellationToken token = default);
    Task RemoveByPrefixAsync(string key, CancellationToken token = default);
}


public class CacheService : ICacheService
{
    private readonly IDistributedCache _cacheProvider;

    public CacheService(IDistributedCache cacheProvider)
    {
        _cacheProvider = cacheProvider;
    }


    public async Task<T?> GetAsync<T>(string key, CancellationToken token = default) where T : class
    {
        string? cacheValue = await _cacheProvider.GetStringAsync(key, token);
        if (cacheValue is null)
            return null;

        T? value = JsonSerializer.Deserialize<T>(cacheValue, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return value;
    }

    public async Task<T?> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken token = default) where T : class
    {
        string? cacheValue = await _cacheProvider.GetStringAsync(key, token);
        if (cacheValue is not null)
        {
            return JsonSerializer.Deserialize<T>(cacheValue, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        var value = await factory();
        await SetAsync(key, value, token);
        return value;
    }

    public Task SetAsync<T>(string key, T value, CancellationToken token = default) where T : class
    {
        string cacheValue = JsonSerializer.Serialize(value, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        return _cacheProvider.SetStringAsync(key, cacheValue, token);
    }

    public Task RemoveAsync(string key, CancellationToken token = default)
    {
        return _cacheProvider.RemoveAsync(key, token);
    }

    public Task RemoveByPrefixAsync(string key, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }


}

