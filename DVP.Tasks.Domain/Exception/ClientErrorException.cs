namespace DVP.Tasks.Domain.Exception;

public abstract class ClientErrorException : System.Exception
{

    public string Details { get; }
    public int StatusCode { get; }
    public string Code { get; }

    public ClientErrorException()
    {

    }
    protected ClientErrorException(int statusCode)
    {
        StatusCode = statusCode;
    }

    protected ClientErrorException(string message) : base(message) { }

    /// <summary>
    /// Exception for 4XX Errors
    /// </summary>
    /// <param name="statusCode"></param>
    /// <param name="message"></param>
    /// <param name="details"></param>
    protected ClientErrorException(int statusCode, string message, string details = null) : base(message)
    {
        StatusCode = statusCode;
        Details = details;
    }

    protected ClientErrorException(int status, string message, string details = null, string code = null) : base(message)
    {
        StatusCode = status;
        Details = details;
        Code = code;
    }
}