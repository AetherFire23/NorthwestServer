using System.Collections.Concurrent;
using System.Reflection;
namespace WebAPI.GameTasks;

public static class GameTaskTypeSelector
{
    public static List<Type> RegisteredGameTasks { get; private set; } = [];

    private static readonly IReadOnlyDictionary<GameTaskCodes, Type> _gameTasksMap = CreateTaskTypesMap();

    public static Type GetGameTaskType(GameTaskCodes taskCode)
    {
        if (!_gameTasksMap.ContainsKey(taskCode))
        {
            throw new NotImplementedException($"The task code '{taskCode}' is not implemented.");
        }

        return _gameTasksMap[taskCode];
    }

    private static ConcurrentDictionary<GameTaskCodes, Type> CreateTaskTypesMap()
    {
        Dictionary<GameTaskCodes, Type> gameTasksMap = new Dictionary<GameTaskCodes, Type>();

        foreach (Type gameTaskType in GetTaskTypes())
        {
            var attr = CustomAttributeExtensions.GetCustomAttribute<GameTaskAttribute>(gameTaskType);

            gameTasksMap.Add(attr.TaskCode, gameTaskType);

            // initialize the field of GameTaskCode
        }

        return new ConcurrentDictionary<GameTaskCodes, Type>(gameTasksMap);
    }

    public static List<Type> GetTaskTypes()
    {
        List<Type> gameTaskTypes = typeof(Program).Assembly.GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract
            && typeof(IGameTask).IsAssignableFrom(type)
            && CustomAttributeExtensions.GetCustomAttribute<GameTaskAttribute>(type) != null).ToList();

        if (!RegisteredGameTasks.Any())
        {
            RegisteredGameTasks.AddRange(gameTaskTypes);
        }

        return gameTaskTypes;
    }
}
