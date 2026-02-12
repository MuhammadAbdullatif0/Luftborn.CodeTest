namespace Application.Common;

public readonly struct Unit { }

public sealed class ServiceResult<T>
{
    public bool IsSuccess { get; }
    public T? Data { get; }
    public string? Error { get; }
    public ServiceResultKind Kind { get; }
    public int? CreatedId { get; }

    private ServiceResult(bool isSuccess, T? data, string? error, ServiceResultKind kind, int? createdId = null)
    {
        IsSuccess = isSuccess;
        Data = data;
        Error = error;
        Kind = kind;
        CreatedId = createdId;
    }

    public static ServiceResult<T> Ok(T data) => new(true, data, null, ServiceResultKind.Ok);
    public static ServiceResult<T> Created(T data, int id) => new(true, data, null, ServiceResultKind.Created, id);
    public static ServiceResult<T> NotFound() => new(false, default, null, ServiceResultKind.NotFound);
    public static ServiceResult<T> BadRequest(string error) => new(false, default, error, ServiceResultKind.BadRequest);
}

public enum ServiceResultKind { Ok, Created, NotFound, BadRequest }
