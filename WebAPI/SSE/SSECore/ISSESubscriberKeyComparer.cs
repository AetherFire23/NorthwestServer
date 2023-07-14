using Shared_Resources.Models.SSE;

namespace WebAPI.SSE.SSECore;

public class ISSESubscriberKeyComparer<T> : IEqualityComparer<T> where T : ISSESubscriber
{
    public bool Equals(T x, T y)
    {
        if (ReferenceEquals(x, y))
            return true;
        if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            return false;
        return x.Id.Equals(y.Id);
    }

    public int GetHashCode(T obj)
    {
        return obj.Id.GetHashCode();
    }
}
