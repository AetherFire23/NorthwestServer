using System.Diagnostics;

namespace WebAPI.SSE;

public static class SSEServiceRegistar
{
    public static void RegisterSSEManagers(this IServiceCollection services)// config
    {
        var types = DiscoverSSEClientManagerTypesInAssembly();
        foreach (Type type in types)
        {
            services.AddSingleton(type);
        }
        Trace.WriteLine("SSE Managers registered");
    }

    public static T GetSSEManager<T>(this IServiceProvider serviceProvider) where T : IClientManager // rretireve
    {
        object? sseManager = serviceProvider.GetService(typeof(T));
        if (sseManager is null) throw new Exception("Could not find specified SSE manager");

        T manager = (T)sseManager;
        return manager;
    }

    private static List<Type> DiscoverSSEClientManagerTypesInAssembly()
    {
        var types = typeof(IClientManager).Assembly.GetTypes()
            .Where(x => !x.IsAbstract && x.IsClass
            && typeof(IClientManager).IsAssignableFrom(x)).ToList();

        return types;
    }
}
