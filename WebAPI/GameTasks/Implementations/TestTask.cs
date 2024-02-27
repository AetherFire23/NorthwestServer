using WebAPI.Enums;
using WebAPI.Models;
using WebAPI.Repositories;
namespace WebAPI.GameTasks.Implementations;

[GameTask(GameTaskCodes.TestTask)]
public class TestTask : GameTaskBase
{
    private readonly StationRepository _stationRepository;
    public TestTask(StationRepository stationRepository)
    {
        _stationRepository = stationRepository;
    }

    public override async Task<GameTaskAvailabilityResult> DetermineAvailability(GameState context)
    {
        var gameTaskAvailabilityResult = new GameTaskAvailabilityResult("GameTask 1 ");

        gameTaskAvailabilityResult.Requirements.Add(ValidateItemRequirement(context));
        return gameTaskAvailabilityResult;
    }

    public override async Task Execute(GameTaskExecutionContext context)
    {
        _stationRepository.RetrieveStationAsync()
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
