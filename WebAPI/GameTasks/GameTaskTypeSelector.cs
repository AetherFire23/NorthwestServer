using Shared_Resources.GameTasks;
using System.Collections.Concurrent;
using System.Reflection;
using WebAPI.GameTasks;

namespace WebAPI.GameTasks
{
    public static class GameTaskTypeSelector
    {
        // ah holy shit javais pas realize que le execution au start du program se passait sur les static functions
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
                var attr = CustomAttributeExtensions.GetCustomAttribute<GameTaskAttribute>(gameTaskType);

                gameTasksMap.Add(attr.TaskCode, gameTaskType);
            }

            return new ConcurrentDictionary<GameTaskCodes, Type>(gameTasksMap);
        }

        private static List<Type> GetTaskTypes()
        {
            var types = typeof(Program).Assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract
                && typeof(IGameTask).IsAssignableFrom(type)
                && CustomAttributeExtensions.GetCustomAttribute<GameTaskAttribute>(type) != null).ToList();
            return types;
        }
    }
}
