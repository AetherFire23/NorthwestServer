using Shared_Resources.Models.SSE;

namespace WebAPI.SSE.SSECore;

/// <summary> Simple subscriber if you dont need additional props</summary>
public class SSESubscriber : ISSESubscriber
{
    public Guid Id { get; set; }
    public SSESubscriber(Guid id)
    {
        Id = id;
    }
}
