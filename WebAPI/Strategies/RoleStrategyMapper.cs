using System.Collections.Concurrent;
using System.Reflection;
using WebAPI.Enums;
using WebAPI.Interfaces;

namespace WebAPI.Strategies;

public static class RoleStrategyMapper
{
    public static IReadOnlyDictionary<RoleType, Type> RoleStrategyTypeMap = CreateRoleStratMap();

    public static Type GetStrategyTypeByRole(RoleType role)
    {
        if (!RoleStrategyTypeMap.ContainsKey(role)) throw new Exception($"Strategy not found for role: {role}");

        Type strategyType = RoleStrategyTypeMap[role];
        return strategyType;
    }

    private static IReadOnlyDictionary<RoleType, Type> CreateRoleStratMap()
    {
        List<Type> types = DiscoverStrategyTypesInAssembly();

        Dictionary<RoleType, Type> map = new Dictionary<RoleType, Type>();
        foreach (Type type in types)
        {
            RoleType roleType = CustomAttributeExtensions.GetCustomAttribute<RoleStrategyAttribute>(type).RoleType;
            map.Add(roleType, type);
        }
        ConcurrentDictionary<RoleType, Type> mapConcurrent = new ConcurrentDictionary<RoleType, Type>(map);
        return mapConcurrent;
    }

    private static List<Type> DiscoverStrategyTypesInAssembly()
    {
        List<Type> types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => x.IsClass && !x.IsAbstract
              && typeof(IRoleInitializationStrategy).IsAssignableFrom(x)).ToList();

        // should have customAttributes
        bool hasAllCustomAttributes = types.Any(x => CustomAttributeExtensions.GetCustomAttribute<RoleStrategyAttribute>(x) is null);
        if (hasAllCustomAttributes) throw new Exception("All role strategies must have a RoleTypeAttribute");

        return types;
    }

    public static void RegisterRoleStrategies(WebApplicationBuilder builder)
    {
        List<Type> types = DiscoverStrategyTypesInAssembly();

        foreach (Type type in types)
        {
            _ = builder.Services.AddTransient(type);
        }
    }
}
