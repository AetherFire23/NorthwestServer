using Shared_Resources.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

// unity : Use GetFullEndpoint(typeof(MainMenuEndpoits), MainMenuEndpoints.State)
namespace Shared_Resources.Constants.Endpoints;

public static class EndpointPathsMapper
{
    // Type is classes with [EndPointPathsMapper] attribute
    // Dictionary is uppercase endpoint name ("State") mapped with full path ("localhost:7060/mainmenu/state")
    public static Dictionary<Type, Dictionary<string, string>> EndpointTypesAndMap = CreateControllerAndEndpointsMapping();

    public static string GetFullEndpoint(Type controllerType, string endpoint)
    {
        var controllerEndpointsMapping = EndpointTypesAndMap.GetValueOrDefault(controllerType) ?? throw new Exception($"Did not find controller :{controllerType}");
        var fullPath = controllerEndpointsMapping.GetValueOrDefault(endpoint) ?? throw new Exception($"Did not find endpoint : {endpoint}");
        return fullPath;
    }

    public static Dictionary<Type, Dictionary<string, string>> CreateControllerAndEndpointsMapping()
    {
        var allTypes = typeof(EndpointPathsMapper).Assembly.GetTypes();
        var controllerTypes = allTypes.Where(ReflectionHelper.HasCustomAttributeFilter<ControllerPathMapperAttribute>).ToList();

        var controllerMapping = new Dictionary<Type, Dictionary<string, string>>();
        foreach (var controllerType in controllerTypes)
        {
            var endpointMapping = CreateEndpointMappings(controllerType);
            controllerMapping.Add(endpointMapping.Key, endpointMapping.Value);
        }

        return controllerMapping;
    }

    // partial name to full name (for Unity-side retrieval)
    public static KeyValuePair<Type, Dictionary<string, string>> CreateEndpointMappings(Type endpointType) // besoin de savoir cest quoi le path jusqua ce endpoint-la, dans le attribute i Guess ???
    {
        var controllerPath = endpointType.GetCustomAttribute<ControllerPathMapperAttribute>().ControllerName;
        var endpointNamesInControllerClass = endpointType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic)
            .Where(x => x != null)
            .Select(x => x.GetValue(null) as string).ToList() ?? new List<string>();

        Dictionary<string, string> partialToFullMapping = new Dictionary<string, string>();
        foreach (var endpointName in endpointNamesInControllerClass)
        {
            partialToFullMapping.Add(endpointName, controllerPath + PrefixObliqueAndLowercase(endpointName));
        }
        var endpointTypeMap = new KeyValuePair<Type, Dictionary<string, string>>(endpointType, partialToFullMapping);
        return endpointTypeMap;
    }

    public static string PrefixObliqueAndLowercase(string endpoint)
    {
        if (endpoint.Contains("/")) throw new Exception($"Cannot already contain oblique {endpoint}");
        string formatted = $"/{endpoint.ToLower()}"; ;
        return formatted;
    }
}
