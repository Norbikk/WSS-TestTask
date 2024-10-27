namespace TaskWSS.Exceptions;

public class NotFoundException : BusinessException
{
    public int Id { get; }

    public NotFoundException() { }

    public NotFoundException(string message) : base(message) { }

    public NotFoundException(string message, int id) : base(message) => Id = id;

    public NotFoundException(string message, Exception inner) : base(message, inner) { }

    public NotFoundException(string message, int id, Exception inner) : base(message, inner) => Id = id;
}