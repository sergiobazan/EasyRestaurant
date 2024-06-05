using Application.Abstractions;
using Infraestructure.Resolvers;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Infraestructure.Services;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;
    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        ContractResolver = new PrivateConstructorContractResolver()
    };

    public CacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        string? cacheValue = await _distributedCache.GetStringAsync(key, cancellationToken);

        if (cacheValue is null) return null;

        T? value = JsonConvert.DeserializeObject<T>(cacheValue, SerializerSettings);

        return value;
    }

    public async Task<T?> GetAsync<T>(string key, Func<Task<T?>> factory, CancellationToken cancellationToken = default) where T : class
    {
        T? cacheValue = await GetAsync<T>(key, cancellationToken);

        if (cacheValue is not null)
        {
            return cacheValue;
        }

        var result = await factory();

        if (result is null) return null;

        await SetAsync(key, result, cancellationToken);

        return result;
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);
    }

    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
    {
        await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(value), cancellationToken);
    }
}
