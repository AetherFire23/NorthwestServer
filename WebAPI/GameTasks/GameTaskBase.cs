using WebAPI.Models;
namespace WebAPI.GameTasks;

/// <summary> Is registered as Transientt </summary>
public abstract class GameTaskBase : IGameTask
{
    public List<Func<GameTaskExecutionContext, TaskRequirement>> dels = [];
    protected GameTaskBase()
    {
    }

    public async Task<GameTaskAvailabilityResult> GetTaskAvailabilityResult(GameState context)
    {
        if (!(await IsVisible(context)))
        {
            return new GameTaskAvailabilityResult()
            {
                IsVisible = false,
            };
        }
        else
        {
            return await DetermineAvailability(context);
        }
    }

    /// <summary> 
    /// This method is the full-blown validation With the task parameters.
    /// /n The idea is that the chosen task parameters could be outside the range of valid targets.
    /// As long as we don't know the player's selections we can not validate the task execution AND its availability.
    /// </summary>
    public abstract Task<GameTaskValidationResult> Validate(GameTaskExecutionContext context);

    /// <summary> This is jsut to check if task is available </summary>
    public abstract Task<GameTaskAvailabilityResult> DetermineAvailability(GameState context);

    /// <summary>
    /// used by GetTaskAvailabilityResult to check if it is even visible instead of greyed out 
    /// </summary>
    public abstract Task<bool> IsVisible(GameState context);

    public abstract Task Execute(GameTaskExecutionContext context);
}
