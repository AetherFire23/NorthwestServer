using Shared_Resources.Enums;
using Shared_Resources.GameTasks;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;
using WebAPI.GameTasks;
using WebAPI.UniversalSkills;

namespace WebAPI.Strategies
{
    public static class SkillStrategyMapper
    {
        public static IReadOnlyDictionary<SkillEnum, Type> RoleStrategyTypeMap = CreateSkillStratMap();

        public static List<Type> GetAllUniversalSkillTypes()
        {
            var skillTypes = RoleStrategyTypeMap.Select(x => x.Value).ToList();
            return skillTypes;
        }

        public static Type GetStrategyTypeBySkill(SkillEnum role)
        {
            if (!RoleStrategyTypeMap.ContainsKey(role)) throw new Exception($"Strategy not found for role: {role}");

            var strategyType = RoleStrategyTypeMap[role];
            return strategyType;
        }

        private static IReadOnlyDictionary<SkillEnum, Type> CreateSkillStratMap()
        {
            var types = DiscoverStrategyTypesInAssembly();

            Dictionary<SkillEnum, Type> map = new Dictionary<SkillEnum, Type>();
            foreach (var type in types)
            {
                SkillEnum roleType = CustomAttributeExtensions.GetCustomAttribute<UniversalSkillAttribute>(type).SkillAttribute;
                map.Add(roleType, type);
            }
            var mapConcurrent = new ConcurrentDictionary<SkillEnum, Type>(map);
            return mapConcurrent;
        }

        private static List<Type> DiscoverStrategyTypesInAssembly()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract
                  && typeof(IUniversalSkill).IsAssignableFrom(x)).ToList();

            // should have customAttributes
            bool hasAllCustomAttributes = types.Any(x => CustomAttributeExtensions.GetCustomAttribute<UniversalSkillAttribute>(x) is null);
            if (hasAllCustomAttributes) throw new Exception("All role strategies must have a RoleTypeAttribute");

            return types;
        }

        public static void RegisterSkillStrategies(WebApplicationBuilder builder)
        {
            var types = DiscoverStrategyTypesInAssembly();

            foreach (var type in types)
            {
                builder.Services.AddTransient(type);
            }
        }
    }
}
