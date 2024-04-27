using Newtonsoft.Json;
using Northwest.Domain.Models;
using Northwest.Domain.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Northwest.Domain.GameTasks.Implementations;

[GameTask(GameTaskCodes.TestTaskInvisible)]
public class TestTaskInvisible : GameTaskBase
{
    public override string Name => "Invisible task";

    [Required]
    public override GameTaskCodes GameTaskCode => GameTaskCodes.TestTaskInvisible;

    private readonly StationRepository _stationRepository;

    public TestTaskInvisible(StationRepository stationRepository)
    {
        _stationRepository = stationRepository;
    }

    #region Availability
    public override async Task<bool> IsVisible(GameState context)
    {
        return false;
    }

    public override async Task<List<TaskRequirement>> GetTaskRequirements(GameState context)
    {
        var requirements = new List<TaskRequirement>
        {
            {GetItemRequirement(context) }
        };
        return requirements;
    }

    public override async Task<List<GameTaskPromptInfo>> GetGameTaskPrompts(GameState context)
    {
        var prompts = new List<GameTaskPromptInfo>();

        var promptInfo = new GameTaskPromptInfo();

        return prompts;
    }

    private List<GameTaskPromptInfo> GetPrompsInfo(GameState gameState)
    {
        var gti = new GameTaskTargetInfo("sex", id: "sds");

        var targets = new List<GameTaskTargetInfo>()
        {
            { new GameTaskTargetInfo()}
        };
        // need to check if valid I guess
        var gameTaskPromptInfo = new GameTaskPromptInfo()
        {
            HasTarget = true,
            MaximumTargets = 1,
            MinimumTargets = 1,
            PromptText = "testTask",
            TaskTargets = targets,
        };

        return new List<GameTaskPromptInfo>() { gameTaskPromptInfo };
    }
    private TaskRequirement GetItemRequirement(GameState context)
    {
        var task = new TaskRequirement
        {
            //FulfillsRequirement = context.PlayerDto.Items.Any(x => x.ItemType == ItemType.Wrench),
            FulfillsRequirement = true,
            Description = "Possesses a Wrench item"
        };

        return task;
    }
    #endregion


    public override async Task Execute(GameTaskExecutionContext context)
    {



        var s = await _stationRepository.RetrieveStationAsync<Station1Property>(context.GameState.GameId, "Wheel");
    }

    public override async Task<GameTaskValidationResult> ValidateExecution(GameTaskExecutionContext context)
    {
        GameTaskValidationResult gameTaskExecutionContext = new GameTaskValidationResult();
        return gameTaskExecutionContext;
    }
}

// parameter type example for Station
// Name : Wheel
// AppearanceName: Wheel
// SerializedValue: 102-sdsd-3434vd
// List

// for room
// Name: MiddleCorridor
// AppearanceName:  Middle Corridor
// SerializedValue: Station 

// for item
// Name: ToolClef
// Tool Clef
// Id

// SerializedValue: {
//  
//}

