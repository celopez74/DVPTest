namespace DVP.Tasks.Domain.Exception;

public class JsonErrorDetail
{
    public string code { get; set; }
    public string detail { get; set; }
}

public class JsonError
{
    public string error_code { get; set; }
    public List<JsonErrorDetail> details { get; set; }
}

public class JsonErrorResponse
{
    public JsonError error { get; set; }
}