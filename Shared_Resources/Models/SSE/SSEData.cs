using Newtonsoft.Json;
using Shared_Resources.Enums;
using System.Text;

namespace Shared_Resources.Models.SSE
{
    public class SSEData<T>
    {
        public SSEEventType EventType { get; set; }
        public T Data { get; set; }

        public SSEData(SSEEventType eventType, T data)
        {
            EventType = eventType;
            Data = data;
        }

        public string ConvertToReadableLine() // this is what gets sent through the server
        {
            var builder = new StringBuilder();
            builder.Append(SSEStrings.EventTypePrefix);
            builder.Append(EventType.ToString());

            builder.Append(SSEStrings.Separator);

            builder.Append(SSEStrings.DataPrefix);
            string serializedData = JsonConvert.SerializeObject(Data);
            builder.Append(serializedData);

            builder.Append(SSEStrings.EndCharacter);
            builder.Append("\n"); // so that it doesnt hand when a reader does ReaderLine()
            
            string line = builder.ToString();
            return line;
        }
    }
}
