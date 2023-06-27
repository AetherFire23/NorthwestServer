using Shared_Resources.Constants;
using Shared_Resources.Entities;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared_Resources.GameTasks
{
    public abstract class GameTaskBase : IGameTask
    {
        public static List<Type> ValidTargetTypes = new List<Type>()
        {
            typeof(Room),
            typeof(Player),
            typeof(Item),
        };

        public abstract GameTaskCodes Code { get; }

        public abstract GameTaskCategory Category { get; }
        public abstract bool HasRequiredConditions(GameState gameState);
        public abstract Task Execute(GameTaskContext context);
        public abstract List<PromptInfo> GetCheckLists(GameState gameState);
        public abstract GameTaskValidationResult Validate(GameTaskContext context);
    }
}
