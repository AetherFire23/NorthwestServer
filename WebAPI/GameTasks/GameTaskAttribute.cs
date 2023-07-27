using Shared_Resources.GameTasks;

namespace WebAPI.GameTasks;

[AttributeUsage(AttributeTargets.Class)]
public class GameTaskAttribute : Attribute
{
    public GameTaskCodes TaskCode;
    public GameTaskAttribute(GameTaskCodes gt)
    {
        TaskCode = gt;
    }
}
