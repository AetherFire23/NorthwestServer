﻿using Newtonsoft.Json;
using Shared_Resources.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared_Resources.Models.SSE
{
    public class SSEClientData
    {
        public SSEEventType EventType { get; set; }
        public string SerializedData { get; set; }

        public SSEClientData(SSEEventType eventType, string serializedData)
        {
            EventType = eventType;
            SerializedData = serializedData;
        }

        public SSEClientData(string line)
        {
            SSEClientData data = SSEClientData.ParseData(line);
            this.EventType = data.EventType;
            this.SerializedData = data.SerializedData;
        }

        public T Deserialize<T>()
        {
            T deserialized = JsonConvert.DeserializeObject<T>(this.SerializedData);
            return deserialized;
        }

        public static SSEClientData ParseData(string sseDataLine)
        {
            // do I need -1 ?
            string eventTypeText = sseDataLine
                .SkipWhile(c => !c.Equals('='))
                .Skip(1)
                .TakeWhile(c => c != SSEStrings.Separator)
                .Aggregate(new StringBuilder(), (sb, c) => sb.Append(c))
                .ToString();
            SSEEventType eventType = (SSEEventType)Enum.Parse(typeof(SSEEventType), eventTypeText);

            var serializedData = sseDataLine
                .SkipWhile(c => !c.Equals(SSEStrings.Separator))
                .Skip(SSEStrings.DataPrefix.Length + 1)
                .TakeWhile(c => !c.Equals(SSEStrings.EndCharacter))
                .Aggregate(new StringBuilder(), (sb, c) => sb.Append(c))
                .ToString();

            //object data = JsonConvert.DeserializeObject<object>(serializedData) ?? throw new Exception("Error while serializing");

            var sseData = new SSEClientData(eventType, serializedData);
            return sseData;
        }
    }
}