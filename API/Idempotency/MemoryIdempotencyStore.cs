using Microsoft.Extensions.Caching.Memory;

namespace API.Idempotency;

public sealed class MemoryIdempotencyStore : IIdempotencyStore
{
    private readonly IMemoryCache _cache;
    private static readonly TimeSpan DefaultTtl = TimeSpan.FromHours(24);

    public MemoryIdempotencyStore(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task<CachedResponse?> GetAsync(string key, CancellationToken cancellationToken = default)
    {
        var cached = _cache.Get<CachedResponse>($"idempotency:{key}");
        return Task.FromResult(cached);
    }

    public Task SetAsync(string key, CachedResponse response, CancellationToken cancellationToken = default)
    {
        _cache.Set($"idempotency:{key}", response, DefaultTtl);
        return Task.CompletedTask;
    }
}
