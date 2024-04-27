using System.Net;

namespace Northwest.WebApi.Exceptions;

public class RequestException : Exception
{
    public string HttpMessage { get; set; } = string.Empty;
    public object? ExceptionData { get; set; } = null;

    public HttpStatusCode StatusCode { get; set; }

    public RequestException(HttpStatusCode statusCode, string message = "(default) Request failed")
    {
        HttpMessage = message;
        StatusCode = statusCode;
    }

    public RequestException(object? obj)
    {
        ExceptionData = obj;
    }
}
