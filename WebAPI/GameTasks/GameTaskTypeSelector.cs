using System.Collections.Concurrent;
using System.Reflection;

namespace WebAPI.GameTasks
{
    public static class GameTaskTypeSelector
    {
        private static readonly IReadOnlyDictionary<GameTaskCode, Type> _gameTasksMap = CreateTaskTypesMap();

        public static Type GetGameTaskType(GameTaskCode taskCode)
        {
            if (!_gameTasksMap.ContainsKey(taskCode))
            {
                throw new NotImplementedException($"The task code '{taskCode}' is not implemented.");
            }

            return _gameTasksMap[taskCode];
        }

        private static ConcurrentDictionary<GameTaskCode, Type> CreateTaskTypesMap()
        {
            var gameTasksMap = new Dictionary<GameTaskCode, Type>();

            foreach (Type gameTaskType in GetTaskTypes())
            {
                var attr = CustomAttributeExtensions.GetCustomAttribute<GameTaskAttribute>(gameTaskType);

                gameTasksMap.Add(attr.TaskCode, gameTaskType);
            }

            return new ConcurrentDictionary<GameTaskCode, Type>(gameTasksMap);
        }

        private static List<Type> GetTaskTypes()
        {
            return typeof(IGameTask).Assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract
                    && typeof(IGameTask).IsAssignableFrom(type)
                    && CustomAttributeExtensions.GetCustomAttribute<GameTaskAttribute>(type) != null)
                .ToList();
        }
    }
}