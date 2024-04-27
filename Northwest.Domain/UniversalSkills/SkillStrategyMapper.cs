using Microsoft.Extensions.DependencyInjection;
using Northwest.Persistence.Enums;
using System.Collections.Concurrent;
using System.Reflection;

namespace Northwest.Domain.UniversalSkills;

public static class SkillStrategyMapper
{
    public static IReadOnlyDictionary<SkillEnum, Type> RoleStrategyTypeMap = CreateSkillStratMap();

    public static List<Type> GetAllUniversalSkillTypes()
    {
        List<Type> skillTypes = RoleStrategyTypeMap.Select(x => x.Value).ToList();
        return skillTypes;
    }

    public static Type GetStrategyTypeBySkill(SkillEnum role)
    {
        if (!RoleStrategyTypeMap.ContainsKey(role)) throw new Exception($"Strategy not found for role: {role}");

        Type strategyType = RoleStrategyTypeMap[role];
        return strategyType;
    }

    private static IReadOnlyDictionary<SkillEnum, Type> CreateSkillStratMap()
    {
        List<Type> types = DiscoverStrategyTypesInAssembly();

        Dictionary<SkillEnum, Type> map = new Dictionary<SkillEnum, Type>();
        foreach (Type type in types)
        {
            SkillEnum roleType = type.GetCustomAttribute<UniversalSkillAttribute>().SkillAttribute;
            map.Add(roleType, type);
        }
        ConcurrentDictionary<SkillEnum, Type> mapConcurrent = new ConcurrentDictionary<SkillEnum, Type>(map);
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
