using Shared_Resources.Models.SSE;
using System.Collections.Concurrent;
using WebAPI.Utils;

namespace Shared_Resources.Models.SSE
{
    public class ClientQueueProcessor
    {
        private readonly HttpResponse _httpResponse;
        private readonly ConcurrentQueue<SSEData> _eventQueue = new ConcurrentQueue<SSEData>();

        public ClientQueueProcessor(HttpResponse response)
        {
            _httpResponse = response;
        }

        public async Task EnqueueEvent(SSEData data)
        {
            _eventQueue.Enqueue(data);

            if(!_eventQueue.Any())
        }

        public async Task ProcessQueue()
        {
            while()
        }
    }
}
