using Microsoft.Extensions.DependencyInjection;

namespace SharedUtils.Extensions;

public static class ServiceProviderExtensions
{
    public static T GetRequiredService<T>(IServiceProvider serviceProvider, Type type) where T : class
    {
        var s = serviceProvider.GetRequiredService(type) as T ?? throw new ArgumentNullException(nameof(type));

        return s;
    }
}