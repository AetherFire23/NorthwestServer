using Newtonsoft.Json;
using Shared_Resources.Enums;
using System.Text;

namespace Shared_Resources.Models.SSE;

public class SSEData
{
    public SSEType EventType { get; set; }
    public string SerializedData { get; set; }

    public SSEData(SSEType eventType, object data)
    {
        EventType = eventType;
        SerializedData = JsonConvert.SerializeObject(data);
    }

    public string ConvertToReadableLine() // this is what gets sent through the server
    {
        var builder = new StringBuilder();
        _ = builder.Append(SSEStrings.EventTypePrefix);
        _ = builder.Append(EventType.ToString());

        _ = builder.Append(SSEStrings.Separator);

        _ = builder.Append(SSEStrings.DataPrefix);
        //string serializedData = JsonConvert.SerializeObject(Data);
        _ = builder.Append(SerializedData);

        _ = builder.Append(SSEStrings.EndCharacter);
        _ = builder.Append("\n"); // so that it doesnt hand when a reader does ReaderLine()

        string line = builder.ToString();
        return line;
    }
}
