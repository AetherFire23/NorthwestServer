using WebAPI.Enums;
using WebAPI.Models;
namespace WebAPI.GameTasks.Implementations;

[GameTask(GameTaskCodes.ChargeCannon)]
public class TestTask2 : GameTaskBase
{
    public override async Task<GameTaskAvailabilityResult> DetermineAvailability(GameState context)
    {
        var gameTaskAvailabilityResult = new GameTaskAvailabilityResult("TestTask2");

        gameTaskAvailabilityResult.Requirements.Add(ValidateItemRequirement(context));
        return gameTaskAvailabilityResult;
    }

    public override async Task Execute(GameTaskExecutionContext context)
    {

    }

    public override async Task<bool> IsVisible(GameState context)
    {
        return true;
    }

    public override async Task<GameTaskValidationResult> Validate(GameTaskExecutionContext context)
    {
        var gameTaskExecutionContext = new GameTaskValidationResult();
        return gameTaskExecutionContext;
    }

    private TaskRequirement ValidateItemRequirement(GameState context)
    {
        var task = new TaskRequirement
        {
            FulfillsRequirement = context.PlayerDTO.Items.Any(x => x.ItemType == ItemType.Wrench),
            Description = "Possesses a Wrench item"
        };

        return task;
    }
}
