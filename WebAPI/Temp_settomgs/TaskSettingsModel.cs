using Shared_Resources.GameTasks;

namespace WebAPI.Temp_settomgs;

public class TaskSettingsModel
{
    public TaskSetting CookSetting = new TaskSetting()
    {
        TaskCode = GameTaskCodes.ChargeCannon,
    };
}
