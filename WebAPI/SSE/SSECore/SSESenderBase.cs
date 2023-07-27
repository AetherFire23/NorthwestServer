namespace WebAPI.SSE;

public class SSESenderBase<T> where T : IClientManager
{
    protected readonly T _client;

    public SSESenderBase(IServiceProvider serviceProvider)
    {
        _client = serviceProvider.GetSSEManager<T>();
    }
}
