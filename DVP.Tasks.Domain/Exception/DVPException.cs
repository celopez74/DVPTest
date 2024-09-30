namespace DVP.Tasks.Domain.Exception;

public class DVPException : System.Exception
{
    public int Code;
    public DVPException() : base() { }
    public DVPException(string message) : base(message) { }

    public DVPException(int code, string message) : base(message)
    {
        Code = code;
    }
}
