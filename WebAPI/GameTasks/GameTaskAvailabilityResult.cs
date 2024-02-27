namespace WebAPI.GameTasks;

public class GameTaskAvailabilityResult
{
    public string GameTaskName { get; set; } = string.Empty;
    public List<TaskRequirement> Requirements { get; set; } = [];
    public string ErrorMessage { get; } = string.Empty;
    public bool IsValid => string.IsNullOrWhiteSpace(ErrorMessage) && IsVisible && this.Requirements.Any(x => !x.FulfillsRequirement);
    public bool IsVisible { get; set; }


    public GameTaskAvailabilityResult()
    {

    }
    public GameTaskAvailabilityResult(string gameTaskName)
    {
        GameTaskName = gameTaskName;
    }

    public GameTaskAvailabilityResult(string gameTaskName, string errorMessage)
    {
        ErrorMessage = errorMessage;
        GameTaskName = gameTaskName;
    }

    public override string ToString() => GameTaskName;
}
