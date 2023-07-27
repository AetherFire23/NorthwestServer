using Shared_Resources.Constants.Mapper;

namespace Shared_Resources.Constants.Endpoints;

[ControllerPathMapper(ServerSideEvents)]
public class SSEEndpoints
{
    public static string GetFullEndpointPath(string endpoint)
    {
        return EndpointPathsMapper.GetFullEndpoint(typeof(SSEEndpoints), endpoint);
    }

    public const string ServerSideEvents = nameof(ServerSideEvents);
    public const string EventStream = nameof(EventStream);
    public const string MainMenuStream = nameof(MainMenuStream);
}
