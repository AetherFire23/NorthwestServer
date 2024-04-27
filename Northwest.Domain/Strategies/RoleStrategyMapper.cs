using Microsoft.Extensions.DependencyInjection;
using Northwest.Domain.Interfaces;
using Northwest.Persistence.Enums;
using System.Collections.Concurrent;
using System.Reflection;

namespace Northwest.Domain.Strategies;

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

        var typesToRoles = types.Select(type =>
        {
            var rt = type.GetCustomAttribute<RoleStrategyAttribute>().RoleType;
            return KeyValuePair.Create(rt, type);
        });

        var mapConcurrent = new ConcurrentDictionary<RoleType, Type>(typesToRoles);
        return mapConcurrent;
    }

    private static List<Type> DiscoverStrategyTypesInAssembly()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => x.IsClass && !x.IsAbstract
              && typeof(IRoleInitializationStrategy).IsAssignableFrom(x)).ToList();

        // should have customAttributes
        bool hasAllCustomAttributes = types.Any(x => x.GetCustomAttribute<RoleStrategyAttribute>() is null);
        if (hasAllCustomAttributes) throw new Exception("All role strategies must have a RoleTypeAttribute");

        return types;
    }

    public static void RegisterRoleStrategies(IServiceCollection serviceCollection)
    {
        var types = DiscoverStrategyTypesInAssembly();

        foreach (Type type in types)
        {
            serviceCollection.AddTransient(type);
        }
    }
}
