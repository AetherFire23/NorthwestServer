namespace Northwest.Domain.GameTasks
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GameTaskAttribute : Attribute
    {
        public GameTaskCodes TaskCode { get; set; }
        public GameTaskCategory Category { get; set; }
        public GameTaskAttribute(GameTaskCodes gt, GameTaskCategory category = GameTaskCategory.None)
        {
            TaskCode = gt;
            Category = category;
        }
    }
}

