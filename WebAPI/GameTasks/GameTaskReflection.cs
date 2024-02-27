using System.Reflection;

namespace WebAPI.GameTasks;

public class GameTaskReflection
{
    public static List<Type> GetGameTaskTypes()
    {
        List<Type> unityTasks = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract
            && typeof(IGameTask).IsAssignableFrom(type)).ToList();

        return unityTasks;
    }

    public static List<IGameTask> CreateGameTaskInstances()
    {
        List<Type> types = GetGameTaskTypes();

        IEnumerable<IGameTask?> instantiated = types.Select(x => Activator.CreateInstance(x) as IGameTask);

        if (instantiated.Any(x => x is null))
        {
            throw new NotImplementedException("An error occurred while instantiating an instance of a task validation");
        }

        return instantiated.ToList();
    }
}
