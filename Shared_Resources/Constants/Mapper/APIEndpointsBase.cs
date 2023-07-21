namespace Shared_Resources.Constants.Mapper;

public static class APIEndpointsBase
{
    // Should read from appsettings
    public const bool IsDevelopment = true;

    public const string APIBase = IsDevelopment
        ? DevelopmentBase
        : ProductionBase;



    private const string HttpVariant = "https://";
    private const string Localhost = "localhost:";
    private const string Port = "7060";

    public const string DevelopmentBase = HttpVariant + Localhost + Port;
    public const string ProductionBase = HttpVariant + "NotSetYet";

}
