using System;

namespace Shared_Resources.Models.SSE;

public class SSESubscriptionOptions
{
    public bool IsMainMenuSSE { get; set; }
    public Guid UserId { get; set; }
    public Guid PlayerId { get; set; }
}
