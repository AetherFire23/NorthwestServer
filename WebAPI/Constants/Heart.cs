using Shared_Resources.Enums;
using Shared_Resources.Models.SSE;

namespace WebAPI.Constants
{
    public static class Heart
    {
        public static readonly SSEData<string> Beat = new SSEData<string>(SSEType.Heartbeat, "");
    }
}
