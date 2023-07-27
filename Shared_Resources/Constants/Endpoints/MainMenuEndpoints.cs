using Shared_Resources.Constants.Mapper;

namespace Shared_Resources.Constants.Endpoints;

// private shouldnt be retrieved NOR used
// should add some ToLower shit in the reflection
// must be constant to be used in another attribute.

[ControllerPathMapper(MainMenu)]
public class MainMenuEndpoints
{
    public static string GetFullEndpointPath(string endpoint)
    {
        return EndpointPathsMapper.GetFullEndpoint(typeof(MainMenuEndpoints), endpoint);
    }

    public const string MainMenu = nameof(MainMenu);
    public const string State = nameof(State);
    public const string CreateLobby = nameof(CreateLobby);
}
