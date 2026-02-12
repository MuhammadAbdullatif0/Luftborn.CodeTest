namespace API.Idempotency;

public interface IIdempotencyStore
{
    Task<CachedResponse?> GetAsync(string key, CancellationToken cancellationToken = default);
    Task SetAsync(string key, CachedResponse response, CancellationToken cancellationToken = default);
}

public sealed record CachedResponse(int StatusCode, IReadOnlyDictionary<string, string> Headers, string Body);
