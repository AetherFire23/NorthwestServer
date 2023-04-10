using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Shared_Resources.GameTasks.Implementations_Unity;

namespace Shared_Resources.GameTasks
{
    public class GameTaskReflection
    {
        public static List<Type> GetGameTaskTypes()
        {
            var unityTasks = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract
                && typeof(IGameTask).IsAssignableFrom(type)).ToList();

            return unityTasks;
        }

        public static List<IGameTask> CreateGameTaskInstances()
        {
            var types = GetGameTaskTypes();

            var instantiated = types.Select(x => Activator.CreateInstance(x) as IGameTask);

            if (instantiated.Any(x => x is null))
            {
                throw new NotImplementedException("An error occurred while instantiating an instance of a task validation");
            }

            return instantiated.ToList();
        }
    }
}
