using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Shared_Resources.Models.SSE;
using System.Collections.Concurrent;
using WebAPI.Utils;

namespace WebAPI.SSE
{
    public class GameSSEClientManager : SSEClientManagerBase<PlayerStruct>
    {
        public List<PlayerStruct> GetLoggedInPlayers(Guid gameId)
        {
            var loggedPlayerIds = Subscribers.Where(x => x.GameId == gameId).ToList();
            return loggedPlayerIds;
        }


        //private ConcurrentDictionary<ISSESubscriber, ClientQueueProcessor>
        // 4 AM]TeBeClone: yeah just don't do that
        //[8:34 AM] TeBeClone: write to a queue that support concurrency
        //[8:34 AM] TeBeClone: and using only a single thread to read the queue and pop item + write to stream properly
        //[8:35 AM]TeBeClone: since SSE is CRLF based for property and 2 CRLF based on new message you need to make sure 1 item of the queue it a full item on the SSE part
    }
}
