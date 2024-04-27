using System.Reflection;

namespace Northwest.Domain.GameTasks.Initialization;

internal static class GameTaskTypeCache
{
    private static IEnumerable<Type> _tasks = [];
    public static IEnumerable<Type> Reflected
    {
        get
        {
            if (!_tasks.Any())
            {
                _tasks = ReflectGameTaskTypes();
            }

            return _tasks;
        }
    }

    private static IEnumerable<Type> ReflectGameTaskTypes()
    {
        var gameTaskTypes = Assembly.GetExecutingAssembly().GetTypes()
        .Where(type => type.IsClass && !type.IsAbstract
        && typeof(IGameTask).IsAssignableFrom(type)
        && type.GetCustomAttribute<GameTaskAttribute>() != null);

        return gameTaskTypes;
    }
}