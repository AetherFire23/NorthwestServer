using System;

namespace Shared_Resources.Constants.Endpoints;

[AttributeUsage(AttributeTargets.Class)]
public class ControllerPathMapperAttribute : Attribute
{
    public string ControllerName = string.Empty;
    public ControllerPathMapperAttribute(string controllerPath) // in /syntax
    {
        if (controllerPath.Contains("/")) throw new Exception($"string already formatted : {controllerPath}. Not supported");
        string formatted = EndpointPathsMapper.PrefixObliqueAndLowercase(controllerPath);
        ControllerName = APIEndpoints.APIBase + formatted;
    }
}
