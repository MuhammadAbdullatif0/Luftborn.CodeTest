namespace API.Models;

public sealed class ApiErrorResponse
{
    public int StatusCode { get; init; }
    public string Message { get; init; } = string.Empty;
    public string? Detail { get; init; }
    public IReadOnlyList<string>? Errors { get; init; }

    public static ApiErrorResponse Create(int statusCode, string message, string? detail = null, IReadOnlyList<string>? errors = null)
        => new()
        {
            StatusCode = statusCode,
            Message = message,
            Detail = detail,
            Errors = errors
        };
}
