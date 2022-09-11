using System;

namespace WebAPI.GameTasks
{
    [AttributeUsage(AttributeTargets.Class)]
    public class GameTaskAttribute : Attribute
    {
        public GameTaskCode TaskCode { get; }

        public GameTaskAttribute(GameTaskCode taskCode)
        {
            TaskCode = taskCode;
        }
    }
}