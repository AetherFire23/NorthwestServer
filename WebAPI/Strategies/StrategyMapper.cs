using Shared_Resources.Enums;
using System.Reflection;

namespace WebAPI.Strategies
{
    public static class StrategyMapper
    {
        public static Dictionary<RoleType, IRoleInitializationStrategy> RoleStrategyMapper = CreateRoleStratMap();
        private static Dictionary<RoleType, IRoleInitializationStrategy> CreateRoleStratMap()
        {

        }

        private static List<Type> GetStrategyTypes()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(x => x.IsClass && !x.IsAbstract
                  && typeof(IRoleInitializationStrategy).IsAssignableFrom(x)).ToList();
                
        }
    }
}
