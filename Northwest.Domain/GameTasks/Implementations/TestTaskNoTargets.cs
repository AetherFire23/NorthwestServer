using Newtonsoft.Json;
using Northwest.Domain.Dtos;
using Northwest.Domain.Models;
using Northwest.Domain.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Northwest.Domain.GameTasks.Implementations;

[GameTask(GameTaskCodes.TestTaskNoTargets)]
public class TestTaskNoTargets : GameTaskBase
{
    public override string Name => "No targets";

    [Required]
    public override GameTaskCodes GameTaskCode => GameTaskCodes.TestTaskNoTargets;

    private readonly StationRepository _stationRepository;

    public TestTaskNoTargets(StationRepository stationRepository)
    {
        _stationRepository = stationRepository;
    }

    #region Availability
    public override async Task<bool> IsVisible(GameState context)
    {
        return await Task.FromResult(true);
    }

    public override async Task<List<TaskRequirement>> GetTaskRequirements(GameState context)
    {
        List<TaskRequirement> requirements = new List<TaskRequirement>
        {
            {GetItemRequirement(context) }
        };
        return requirements;
    }

    public override async Task<List<GameTaskPromptInfo>> GetGameTaskPrompts(GameState context)
    {
        List<GameTaskPromptInfo> prompts = new List<GameTaskPromptInfo>();

        return prompts;
    }

    private TaskRequirement GetItemRequirement(GameState context)
    {
        TaskRequirement task = new TaskRequirement
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
        StationDTO s = await _stationRepository.RetrieveStationAsync<Station1Property>(context.GameState.GameId, "Wheel");
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

