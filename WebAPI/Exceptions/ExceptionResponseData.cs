namespace Northwest.WebApi.Exceptions;
public class ExceptionResponseData
{
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; } = null;
}
