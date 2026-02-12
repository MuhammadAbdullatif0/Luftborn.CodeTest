namespace API.Idempotency;

public sealed class IdempotencyMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly HashSet<string> ModifyingMethods = ["POST", "PUT", "PATCH"];

    public IdempotencyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IIdempotencyStore store)
    {
        var key = context.Request.Headers["Idempotency-Key"].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(key) || !ModifyingMethods.Contains(context.Request.Method))
        {
            await _next(context);
            return;
        }

        var cached = await store.GetAsync(key, context.RequestAborted);
        if (cached is not null)
        {
            context.Response.StatusCode = cached.StatusCode;
            foreach (var (name, value) in cached.Headers)
                context.Response.Headers[name] = value;
            await context.Response.WriteAsync(cached.Body);
            return;
        }

        var originalBody = context.Response.Body;
        await using var buffer = new MemoryStream();
        context.Response.Body = buffer;

        try
        {
            await _next(context);
        }
        finally
        {
            context.Response.Body = originalBody;
        }

        buffer.Position = 0;
        var body = await new StreamReader(buffer).ReadToEndAsync();
        var headers = context.Response.Headers
            .Where(h => string.Equals(h.Key, "Content-Type", StringComparison.OrdinalIgnoreCase))
            .ToDictionary(h => h.Key, h => h.Value.ToString());

        await store.SetAsync(key, new CachedResponse(context.Response.StatusCode, headers, body));

        await context.Response.WriteAsync(body);
    }
}
