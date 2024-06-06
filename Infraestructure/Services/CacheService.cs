using Application.Abstractions;
using Infraestructure.Resolvers;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Infraestructure.Services;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;
    private readonly ConcurrentDictionary<string, bool> CacheKeys;
    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        ContractResolver = new PrivateConstructorContractResolver()
    };

    public CacheService(IDistributedCache distributedCache, ConcurrentDictionary<string, bool> cacheKeys)
    {
        _distributedCache = distributedCache;
        CacheKeys = cacheKeys;
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

        CacheKeys.TryRemove(key, out _);
    }

    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
    {
        await _distributedCache.SetStringAsync(
            key, 
            JsonConvert.SerializeObject(value),
            new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(10),
            },
            cancellationToken);

        CacheKeys.TryAdd(key, false);
    }

    public async Task RemoveByPartialKey(string partialKey, CancellationToken cancellationToken = default)
    {
        IEnumerable<Task> tasks = CacheKeys.Keys
            .Where(k => k.Contains(partialKey))
            .Select(k => RemoveAsync(k, cancellationToken));

        await Task.WhenAll(tasks);   
    }
}
