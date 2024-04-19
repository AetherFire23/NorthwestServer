using Northwest.Models;
namespace Northwest.GameTasks;

/// <summary> Is registered as Transientt </summary>
public abstract class GameTaskBase : IGameTask
{
    public List<Func<GameTaskExecutionContext, TaskRequirement>> dels = [];
    protected GameTaskBase()
    {
    }

    // Order of execution:
    // 1. Is visible
    // 2. Get task requirements
    // 3. Get prompts info

    public abstract string Name { get; }

    // initialized through reflection
    public abstract GameTaskCodes GameTaskCode { get; }

    /// <summary>
    /// used by GetTaskAvailabilityResult to check if it is even visible instead of greyed out 
    /// </summary>
    public abstract Task<bool> IsVisible(GameState context);


    /// <summary> This is jsut to check if task is available </summary>
    public abstract Task<List<TaskRequirement>> GetTaskRequirements(GameState context);

    public abstract Task<List<GameTaskPromptInfo>> GetGameTaskPrompts(GameState context);




    /// <summary> 
    /// This method is the full-blown validation With the task parameters.
    /// /n The idea is that the chosen task parameters could be outside the range of valid targets.
    /// As long as we don't know the player's selections we can not validate the task execution AND its availability.
    /// </summary>
    public abstract Task<GameTaskValidationResult> ValidateExecution(GameTaskExecutionContext context);
    public abstract Task Execute(GameTaskExecutionContext context);


}
