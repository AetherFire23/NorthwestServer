using System.ComponentModel.DataAnnotations;
namespace Northwest.Domain.GameTasks;

public class GameTaskAvailabilityResult
{
    // Can I see the task ? lets just not send the task if not visible.
    // Can I see the requirements of the task ?
    // A way I can see the targets and list of targets


    [Required]
    public string GameTaskName { get; set; } = string.Empty;

    [Required]
    public GameTaskCodes GameTaskCode { get; set; }

    [Required]
    public List<TaskRequirement> Requirements { get; set; } = [];

    [Required]
    public List<GameTaskPromptInfo> TaskPromptInfos { get; set; } = [];

    // Set by repository if all taskrquirements are fulfilled
    public bool CanExecuteTask => Requirements.All(t => t.FulfillsRequirement);


    public GameTaskAvailabilityResult()
    {

    }

    public GameTaskAvailabilityResult(string gameTaskName)
    {
        GameTaskName = gameTaskName;
    }

    public GameTaskAvailabilityResult(string gameTaskName, List<TaskRequirement> taskRequirements)
    {
        Requirements = taskRequirements;
        GameTaskName = gameTaskName;
    }

    public override string ToString() => GameTaskName;
}

public class GameTaskTargetInfo
{
    public Guid Id { get; set; } = Guid.Empty;
    public string AppearanceName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

    public GameTaskTargetInfo()
    {

    }

    public GameTaskTargetInfo(string appearanceName, string name = "", string id = "")
    {
        AppearanceName = appearanceName;

        if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(id)) throw new Exception("Both Id and Name cannot be null.");

        Name = name;


        Id = string.IsNullOrEmpty(id) ? Guid.Empty : new Guid(id);
    }
}

public class GameTaskPromptInfo
{
    public string PromptText { get; set; } = string.Empty;
    public bool HasTarget { get; set; } = false;
    [Required]
    public List<GameTaskTargetInfo> TaskTargets { get; set; } = [];
    public int MaximumTargets { get; set; } = 0;
    public int MinimumTargets { get; set; } = 0;
}