using Newtonsoft.Json;
using Northwest.Domain.Dtos;
using Northwest.Domain.Models;
using Northwest.Domain.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Northwest.Domain.GameTasks.Implementations;

[GameTask(GameTaskCodes.TestTaskWithTargets)]
public class TaskTaskMultipleTargets : GameTaskBase
{
    public override string Name => "Multiple Targets";

    [Required]
    public override GameTaskCodes GameTaskCode => GameTaskCodes.TestTaskWithTargets;

    private readonly StationRepository _stationRepository;

    public TaskTaskMultipleTargets(StationRepository stationRepository)
    {
        _stationRepository = stationRepository;
    }

    #region Availability
    public override async Task<bool> IsVisible(GameState context)
    {
        return true;
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

        return GetPrompsInfo(context);
    }

    private List<GameTaskPromptInfo> GetPrompsInfo(GameState gameState)
    {
        var gti = new GameTaskTargetInfo("appearance name", "test");

        var targets = new List<GameTaskTargetInfo>()
        {
            { new GameTaskTargetInfo()
                {
                    AppearanceName = "myTarget",
                    Name = "target 1 "
                }
            },
            { new GameTaskTargetInfo()
                {
                    AppearanceName = "myTarget2",
                    Name = "target 2"
                }
            },
        };
        var targets2 = new List<GameTaskTargetInfo>()
        {
            { new GameTaskTargetInfo()
                {
                    AppearanceName = "target 2, - screen 2",
                    Name = "target 1 - screen 2 "
                }
            },
            { new GameTaskTargetInfo()
                {
                    AppearanceName = "target 2, - screen 2",
                    Name = "target 2, - screen 2"
                }
            },
        };
        // need to check if valid I guess
        var firstTargetSelectionScreen = new GameTaskPromptInfo()
        {
            HasTarget = true,
            MaximumTargets = 1,
            MinimumTargets = 1,
            PromptText = "testTask",
            TaskTargets = targets,
        };

        var secondTargetSelectionScreen = new GameTaskPromptInfo()
        {
            HasTarget = true,
            MaximumTargets = 1,
            MinimumTargets = 1,
            PromptText = "testTask2",
            TaskTargets = targets2,
        };

        return new List<GameTaskPromptInfo>()
        {
            firstTargetSelectionScreen,
            secondTargetSelectionScreen,
        };
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

