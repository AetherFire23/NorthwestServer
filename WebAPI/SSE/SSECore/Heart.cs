using Shared_Resources.Enums;
using Shared_Resources.Models.SSE;

namespace WebAPI.SSE.SSECore;

public static class Heart
{
    public static readonly SSEData Beat = new SSEData(SSEType.Heartbeat, "");
}
