namespace TaskWSS.Exceptions;

/// <summary>
/// базовый класс для кастомный исключений (чтобы отличать системные исключения от собственноручных)
/// </summary>
public abstract class BusinessException : Exception
{
    protected BusinessException() { }

    protected BusinessException(string message) : base(message) { }

    protected BusinessException(string message, Exception inner) : base(message, inner) { }
}