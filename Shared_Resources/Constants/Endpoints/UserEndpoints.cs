namespace Shared_Resources.Constants.Endpoints;

[ControllerPathMapper(Users)]
public class UserEndpoints
{
    public const string Users = nameof(Users);
    public const string Login = nameof(Login);
    public const string Register = nameof(Register);

    public static string GetFullEndpointPath(string endpoint)
    {
        return EndpointPathsMapper.GetFullEndpoint(typeof(UserEndpoints), endpoint);
    }
}
