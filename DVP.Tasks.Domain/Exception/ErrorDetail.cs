using System.Reflection;

namespace DVP.Tasks.Domain.Exception;

public class ErrorDetail
{
    private string source;
    public string Source { get => source.Split(".").Last(); set => source = value; }
    public string Code { get; set; }
    public string Message { get; set; }
    public List<string> Params { get; set; }
    public List<string> Detail { get; set; }

    public ErrorDetail()
    {
        Params = new List<string>();
        Source = Assembly.GetEntryAssembly().GetName().Name;
    }
}
