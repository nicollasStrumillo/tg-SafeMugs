namespace backend.Exceptions;

public class DBException : Exception
{
    public DBException(string message) : base(message)
    {
    }
}
