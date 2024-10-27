namespace TaskWSS.Operations;

public enum StatusOperation
{
    Success,
    Exception,
    NotFound
}

/// <summary>
/// Интерфейс результата выполенения операции
/// </summary>S
public interface IOperationResult
{
    StatusOperation Status { get; }
    Exception Exception { get; }
}

public interface IOperationResult<out T> : IOperationResult
{
    T Result { get; }
}