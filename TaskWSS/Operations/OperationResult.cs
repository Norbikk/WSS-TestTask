
namespace TaskWSS.Operations;

public struct OperationResult : IOperationResult
{
    public StatusOperation Status { get; }
    public Exception Exception { get; }

    public OperationResult(StatusOperation status)
    {
        Status = status;
        Exception = null;
    }

    public OperationResult(Exception exception)
    {
        Status = StatusOperation.Exception;
        Exception = exception ?? throw new ArgumentNullException(nameof(exception));
    }
    public OperationResult(StatusOperation status, Exception exception)
    {
        Status = status;
        Exception = exception ?? throw new ArgumentNullException(nameof(exception));
    }

    public static OperationResult Success()
        => new(StatusOperation.Success);

    public static OperationResult<T> Success<T>(T result)
        => new(result);

    public static OperationResult NotFound()
        => new(StatusOperation.NotFound);
    public static OperationResult NotFound(Exception exception)
        => new(StatusOperation.NotFound, exception);

    public static OperationResult<T> NotFound<T>(T result = default)
        => new(StatusOperation.NotFound, result);
    public static OperationResult<T> NotFound<T>(Exception exception, T result = default)
        => new(StatusOperation.NotFound, exception, result);
    
    public static OperationResult Error(Exception exception)
        => new(exception);

    public static OperationResult<T> Error<T>(Exception exception)
        => new(exception);

    public static implicit operator OperationResult(Exception exception)
        => new(exception);
}

public struct OperationResult<T> : IOperationResult<T>
{
    public T Result { get; }
    public StatusOperation Status { get; }
    public Exception Exception { get; }

    public OperationResult(T result)
    {
        Status = StatusOperation.Success;
        Exception = null;
        Result = result;
    }

    public OperationResult(StatusOperation status, T result) : this(result)
    {
        Status = status;
        Exception = null;
        Result = result;
    }

    public OperationResult(Exception exception)
    {
        Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        Status = StatusOperation.Exception;
        Result = default;
    }
    public OperationResult(StatusOperation status, Exception exception, T result)
    {
        Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        Status = status;
        Result = result;
    }

    public static implicit operator OperationResult<T>(T result)
        => new(result);

    public static implicit operator OperationResult<T>(Exception exception)
        => new(exception);

    public static implicit operator OperationResult<T>(OperationResult result)
    {
        return new OperationResult<T>(result.Status, result.Exception, default);
    }
}