namespace DVP.Tasks.Domain.Exception;

public class ErrorResponse
{
    public int Status { get; set; }
    public string Service { get; set; }
    public List<ErrorDetail> Errors { get; set; }
}
