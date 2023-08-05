using Shared_Resources.Enums;
using System.Collections.Concurrent;
using System.Reflection;
using WebAPI.Interfaces;

namespace WebAPI.Strategies;

public static class RoleStrategyMapper
{
    public static IReadOnlyDictionary<RoleType, Type> RoleStrategyTypeMap = CreateRoleStratMap();

    public static Type GetStrategyTypeByRole(RoleType role)
    {
        if (!RoleStrategyTypeMap.ContainsKey(role)) throw new Exception($"Strategy not found for role: {role}");

        var strategyType = RoleStrategyTypeMap[role];
        return strategyType;
    }

    private static IReadOnlyDictionary<RoleType, Type> CreateRoleStratMap()
    {
        var types = DiscoverStrategyTypesInAssembly();

        Dictionary<RoleType, Type> map = new Dictionary<RoleType, Type>();
        foreach (var type in types)
        {
            RoleType roleType = CustomAttributeExtensions.GetCustomAttribute<RoleStrategyAttribute>(type).RoleType;
            map.Add(roleType, type);
        }
        var mapConcurrent = new ConcurrentDictionary<RoleType, Type>(map);
        return mapConcurrent;
    }

    private static List<Type> DiscoverStrategyTypesInAssembly()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => x.IsClass && !x.IsAbstract
              && typeof(IRoleInitializationStrategy).IsAssignableFrom(x)).ToList();

        // should have customAttributes
        bool hasAllCustomAttributes = types.Any(x => CustomAttributeExtensions.GetCustomAttribute<RoleStrategyAttribute>(x) is null);
        if (hasAllCustomAttributes) throw new Exception("All role strategies must have a RoleTypeAttribute");

        return types;
    }

    public static void RegisterRoleStrategies(WebApplicationBuilder builder)
    {
        var types = DiscoverStrategyTypesInAssembly();

        foreach (var type in types)
        {
            _ = builder.Services.AddTransient(type);
        }
    }
}
