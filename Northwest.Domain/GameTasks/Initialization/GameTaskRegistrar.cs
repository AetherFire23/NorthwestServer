using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Northwest.Domain.GameTasks.Initialization;

public static class GameTaskRegistrar
{

    public static void RegisterGameTaskTypes(this IServiceCollection serviceCollection)
    {
        foreach (var taskType in GameTaskTypeCache.Reflected)
        {
            serviceCollection.AddScoped(taskType);
        }

        LogGameTaskRegistrations(GameTaskTypeCache.Reflected);
    }

    private static void LogGameTaskRegistrations(IEnumerable<Type> gameTasks)
    {
        Console.WriteLine($"A total of {gameTasks.Count()} Game Tasks were registered.");
        Console.WriteLine("this includes the following gameTasks:");

        foreach (var type in gameTasks.Select((x, i) => (x, i)))
        {
            Console.WriteLine($"[{type.i}] {type.x.FullName}");
        }
    }
}