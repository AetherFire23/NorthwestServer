using Shared_Resources.GameTasks;

namespace WebAPI.Temp_settomgs;

public class TaskSetting
{
    public Guid Id { get; set; }
    public GameTaskCodes TaskCode { get; set; }
    public string SerializedProperties { get; set; }
}
