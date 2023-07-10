using Shared_Resources.Models.SSE;
using System.Reflection;
using WebAPI.Services;
using WebAPI.Utils;

namespace WebAPI.SSE
{
    public static class SSERegistar
    {
        private static List<Type> GetSSEClientManagerTypes() 
        {
            var types = typeof(IClientManager).Assembly.GetTypes()
                .Where(x => !x.IsAbstract && x.IsClass
                && typeof(IClientManager).IsAssignableFrom(x)).ToList();

            return types;
        }

        public static void RegisterSSEManagerTypes(IServiceCollection services)
        {
            var types = GetSSEClientManagerTypes();
            foreach (Type type in types)
            {
                services.AddSingleton(type);
            }
            Console.WriteLine("SSE Managers registered");
        }
    }


    public static partial class ServiceCollectionExtensions
    {
        public static T GetSSEManager<T>(this IServiceProvider serviceProvider) where T : IClientManager
        {
            object? sseManager = serviceProvider.GetService(typeof(T));
            if (sseManager is null) throw new Exception("Could not find specified SSE manager");

            T manager = (T)sseManager;
            return manager;
        }
    }
}
