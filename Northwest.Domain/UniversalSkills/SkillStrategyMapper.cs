using Microsoft.Extensions.DependencyInjection;
using Northwest.Persistence.Enums;
using System.Collections.Concurrent;
using System.Reflection;

namespace Northwest.Domain.UniversalSkills;

public static class SkillStrategyMapper
{
    public static IReadOnlyDictionary<Skills, Type> RoleStrategyTypeMap = CreateSkillStratMap();

    public static List<Type> GetAllUniversalSkillTypes()
    {
        List<Type> skillTypes = RoleStrategyTypeMap.Select(x => x.Value).ToList();
        return skillTypes;
    }

    public static Type GetStrategyTypeBySkill(Skills role)
    {
        if (!RoleStrategyTypeMap.ContainsKey(role)) throw new Exception($"Strategy not found for role: {role}");

        Type strategyType = RoleStrategyTypeMap[role];
        return strategyType;
    }

    private static IReadOnlyDictionary<Skills, Type> CreateSkillStratMap()
    {
        List<Type> types = DiscoverStrategyTypesInAssembly();

        Dictionary<Skills, Type> map = new Dictionary<Skills, Type>();
        foreach (Type type in types)
        {
            Skills roleType = type.GetCustomAttribute<UniversalSkillAttribute>().SkillAttribute;
            map.Add(roleType, type);
        }
        ConcurrentDictionary<Skills, Type> mapConcurrent = new ConcurrentDictionary<Skills, Type>(map);
        return mapConcurrent;
    }

    private static List<Type> DiscoverStrategyTypesInAssembly()
    {
        List<Type> types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(x => x.IsClass && !x.IsAbstract
              && typeof(ITickedSkills).IsAssignableFrom(x)).ToList();

        // should have customAttributes
        bool hasAllCustomAttributes = types.Any(x => x.GetCustomAttribute<UniversalSkillAttribute>() is null);
        if (hasAllCustomAttributes) throw new Exception("All role strategies must have a RoleTypeAttribute");

        return types;
    }

    public static void RegisterSkillStrategies(IServiceCollection serviceCollection)
    {
        var types = DiscoverStrategyTypesInAssembly();

        foreach (var type in types)
        {
            serviceCollection.AddTransient(type);
        }
    }
}
