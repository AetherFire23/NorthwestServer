namespace Shared_Resources.Models;

public class ClientCallResultGeneric<T>
    where T : class // lets try to make this generic some day
{
    // Successes do not concern http client but client-related actions instead.
    // Therefore all ClientCallResult are successful HTTP requests, but not necessarily successful from the point of view of the client.
    // For example, if a client tries to add a new friend but it already exists.
    public bool IsSuccessful { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Content { get; set; }


    public ClientCallResultGeneric(T? content)
    {
        Content = content;
    }

    public static ClientCallResultGeneric<string> Success => new ClientCallResultGeneric<string>(string.Empty)
    {
        IsSuccessful = true,
        Message = "Success"
    };

    public static ClientCallResultGeneric<string> Failure => new ClientCallResultGeneric<string>(string.Empty)
    {
        IsSuccessful = false,
        Message = "Failed"
    };
}