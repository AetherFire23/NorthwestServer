using System.Reflection;
namespace WebAPI.GameTasks.Reflect;

public class DelegateCache
{
    private readonly IServiceProvider _serviceProvider;

    public Dictionary<Type, List<Func<GameTaskExecutionContext, TaskRequirement>>> GameTasksRequirementMap { get; set; }
    public DelegateCache(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public List<Func<GameTaskExecutionContext, TaskRequirement>> GetGameTaskRequirementDelegates(Type type)
    {
        List<Func<GameTaskExecutionContext, TaskRequirement>> taskRequirementDelegates = GameTasksRequirementMap[type];
        return taskRequirementDelegates;
    }

    public Dictionary<Type, List<Func<GameTaskExecutionContext, TaskRequirement>>> BindDelegatesToGameTaskInstances()
    {
        Dictionary<Type, List<Func<GameTaskExecutionContext, TaskRequirement>>> gameTaskRequirementsMap = new Dictionary<Type, List<Func<GameTaskExecutionContext, TaskRequirement>>>();

        IEnumerable<Type> gameTaskClasses = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => !t.IsAbstract && t.IsClass &&
             typeof(IGameTask).IsAssignableFrom(t));

        // Init the lists so that they're not empty 
        foreach (Type? gametask in gameTaskClasses)
        {
            gameTaskRequirementsMap.Add(gametask, []);
        }

        foreach (Type? gameTask in gameTaskClasses)
        {
            // find the methods with the TaskRequirement Attribute
            List<MethodInfo> taskRequirementMethods = gameTask
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Static | BindingFlags.NonPublic)
                .Where(x => x.GetCustomAttribute<TaskRequirementAttribute>() is not null)
                .ToList();

            // create the delegates and check if it is the wrong return type
            IEnumerable<Func<GameTaskExecutionContext, TaskRequirement>> delegates = taskRequirementMethods.Select(m =>
            {
                if (m.ReturnType != typeof(TaskRequirement))
                {
                    throw new ArgumentException("Methods with the TaskRequiment Attribute should be of return type TaskRequirement");
                }

                Type testt2 = m.GetType();

                // so I need to specify the target of the instance or class
                // null for static if I remember well
                GameTaskBase? instanceToBind = _serviceProvider.GetRequiredService(gameTask) as GameTaskBase;
                Func<GameTaskExecutionContext, TaskRequirement> taskDelegate = m.CreateDelegate<Func<GameTaskExecutionContext, TaskRequirement>>(instanceToBind);
                return taskDelegate;
            });

            gameTaskRequirementsMap[gameTask] = delegates.ToList();
        }


        // save the dels inside the gametasks bases

        foreach (KeyValuePair<Type, List<Func<GameTaskExecutionContext, TaskRequirement>>> item in gameTaskRequirementsMap)
        {
            // keys are the gametasks
            GameTaskBase? gametask = _serviceProvider.GetRequiredService(item.Key) as GameTaskBase;
            gametask.dels = gameTaskRequirementsMap[item.Key];
        }

        return gameTaskRequirementsMap;
    }
}


public static class DelegateCacheExtensions
{

    // weird structure and why it is not ins tatic class:
    // the reason is that I need the instances of the class to be already created before
    // binding the delegates to the instances.
    // So I need to first create the delegate cache + the gametask Types and after I will be able to map the delegates
    public static void AddTaskDelegateCache(this IServiceCollection serviceProvider)
    {
        // serviceProvider.AddSingleton<DelegateCache>();
    }

    public static void InitializeTaskDelegateCache(this WebApplication webapp)
    {
        //using (var scope = webapp.Services.CreateScope())
        //{
        //    var delCache = scope.ServiceProvider.GetRequiredService<DelegateCache>();
        //    delCache.GameTasksRequirementMap = delCache.BindDelegatesToGameTaskInstances();



        //}
    }
}