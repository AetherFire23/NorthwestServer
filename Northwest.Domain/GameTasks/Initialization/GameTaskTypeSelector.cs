using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Reflection;
namespace Northwest.Domain.GameTasks.Initialization;

// should convert this to normal service I guess
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

    public static List<Type> GetTaskTypes()
    {
        var gameTaskTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract
            && typeof(IGameTask).IsAssignableFrom(type)
            && type.GetCustomAttribute<GameTaskAttribute>() != null).ToList();

        if (RegisteredGameTasks.Count == 0)
        {
            RegisteredGameTasks.AddRange(gameTaskTypes);
        }

        return gameTaskTypes;
    }

    private static ConcurrentDictionary<GameTaskCodes, Type> CreateTaskTypesMap()
    {
        Dictionary<GameTaskCodes, Type> gameTasksMap = new Dictionary<GameTaskCodes, Type>();

        foreach (Type gameTaskType in GetTaskTypes())
        {
            var attr = gameTaskType.GetCustomAttribute<GameTaskAttribute>();

            gameTasksMap.Add(attr.TaskCode, gameTaskType);

            // initialize the field of GameTaskCode
        }

        return new ConcurrentDictionary<GameTaskCodes, Type>(gameTasksMap);
    }
}

public class GameTaskSelector2
{
    private readonly ConcurrentDictionary<GameTaskCodes, Type> _gameTasksMap;

    public GameTaskSelector2()
    {
        _gameTasksMap = InitializeTaskTypesMap();
    }

    public Type GetGameTaskType(GameTaskCodes taskCode)
    {
        if (!_gameTasksMap.TryGetValue(taskCode, out var type))
        {
            throw new NotImplementedException($"The task code '{taskCode}' is not implemented.");
        }

        return type;
    }

    private ConcurrentDictionary<GameTaskCodes, Type> InitializeTaskTypesMap()
    {
        var gameTasks = GameTaskTypeCache.Reflected.Select(gameTaskType =>
        {
            var taskCode = gameTaskType.GetCustomAttribute<GameTaskAttribute>().TaskCode;
            var kvp = KeyValuePair.Create(taskCode, gameTaskType);
            return kvp;
        });

        var gameTaskMap = new ConcurrentDictionary<GameTaskCodes, Type>(gameTasks);

        return gameTaskMap;
    }
}