using Shared_Resources.Models.SSE;
using System.Collections.Concurrent;
using WebAPI.Services;
using WebAPI.Utils;
using WebAPI.Utils.Extensions;

namespace Shared_Resources.Models.SSE
{
    public class ClientQueueProcessor
    {
        public HttpResponse HttpResponse;
        private CancellationToken _token => HttpResponse.HttpContext.RequestAborted;
        private readonly ConcurrentQueue<SSEData> _eventQueue = new ConcurrentQueue<SSEData>();
        private ManualResetEventSlim _eventSlim = new ManualResetEventSlim(false);

        public ClientQueueProcessor(HttpResponse response)
        {
            HttpResponse = response;
            Task.Run(ProcessQueue);
        }

        public async Task EnqueueEvent(SSEData data)
        {
            _eventQueue.Enqueue(data);
            _eventSlim.Set();
        }

        public async Task ProcessQueue()
        {
            while (!_token.IsCancellationRequested)
            {
                // to avoid tight loop, block thread if _eventqueue is empty
                if (_eventQueue.IsEmpty)
                {
                    _eventSlim.Reset();
                }

                _eventSlim.Wait(_token);

                if (!_eventQueue.TryDequeue(out SSEData data))
                {
                    Console.WriteLine("failed to dequeue data.");
                    continue;
                }

                await HttpResponse.WriteAndFlushSSEData(data);
            }
        }
    }
}
