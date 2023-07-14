using Newtonsoft.Json;

namespace Shared_Resources.Models;

public class ClientCallResult
{
    public bool IsSuccessful { get; set; }
    public string Message { get; set; } = string.Empty;
    public string SerializedContent { get; set; } = string.Empty;

    public object Content
    {
        set
        {
            SerializedContent = JsonConvert.SerializeObject(value, _serializationSettings);
        }
    }

    private JsonSerializerSettings _serializationSettings = new JsonSerializerSettings()
    {
        PreserveReferencesHandling = PreserveReferencesHandling.All,
    };

    //public void SetSerializedContent<T>(T content)
    //{
    //    _serializedContent = JsonConvert.SerializeObject(content, _serializationSettings);
    //}

    public T DeserializeContent<T>()
    {
        T result = JsonConvert.DeserializeObject<T>(SerializedContent);
        return result;
    }

    public static ClientCallResult Success => new ClientCallResult()
    {
        IsSuccessful = true,
        Message = "Success"
    };

    public static ClientCallResult Failure => new ClientCallResult()
    {
        IsSuccessful = false,
        Message = "Failed"
    };
}