using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace WebAPI.GameTasks;

public static class GameTaskTypeSelector
{
    public static List<Type> RegisteredGameTasks { get; private set; } = [];

    // remember execution of static methods take place when first invoked
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
        var gameTasksMap = new Dictionary<GameTaskCodes, Type>();

        foreach (Type gameTaskType in GetTaskTypes())
        {
            GameTaskAttribute? attr = CustomAttributeExtensions.GetCustomAttribute<GameTaskAttribute>(gameTaskType);

            gameTasksMap.Add(attr.TaskCode, gameTaskType);
        }

        return new ConcurrentDictionary<GameTaskCodes, Type>(gameTasksMap);
    }

    private static List<Type> GetTaskTypes()
    {
        var gameTaskTypes = typeof(Program).Assembly.GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract
            && typeof(IGameTask).IsAssignableFrom(type)
            && CustomAttributeExtensions.GetCustomAttribute<GameTaskAttribute>(type) != null).ToList();

        RegisteredGameTasks.AddRange(gameTaskTypes);

        return gameTaskTypes;
    }
}
