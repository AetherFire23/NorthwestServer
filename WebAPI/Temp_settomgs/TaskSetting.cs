using WebAPI.GameTasks;

namespace WebAPI.Temp_settomgs
{
    public class TaskSetting
    {
        public Guid Id { get; set; }
        public GameTaskCode TaskCode { get; set; }
        public string SerializedProperties { get; set; }
    }
}
