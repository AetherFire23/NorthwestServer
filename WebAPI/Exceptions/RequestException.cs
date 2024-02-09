using System.Net;

namespace WebAPI.Exceptions;

public class RequestException : Exception
{
    public string HttpMessage { get; set; } = string.Empty;
    public object? ExceptionData { get; set; } = null;

    public HttpStatusCode StatusCode { get; set; }

    public RequestException(HttpStatusCode statusCode, string message = "(default) Request failed")
    {
        this.HttpMessage = message;
        this.StatusCode = statusCode;
    }

    public RequestException(object? obj)
    {
        this.ExceptionData = obj;
    }
}
