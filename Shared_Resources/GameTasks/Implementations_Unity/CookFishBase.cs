using Shared_Resources.Constants;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared_Resources.GameTasks.Implementations_Unity
{
    public class CookFishBase : GameTaskBase
    {
        public override GameTaskCodes Code => GameTaskCodes.CookFish;

        public override GameTaskCategory Category => GameTaskCategory.Room;

        public override bool HasRequiredConditions(GameState gameState)
        {
            bool isInValidRoom = gameState.Room.Name == nameof(RoomsTemplate.Galley);
            return isInValidRoom;
        }

        public override List<PromptInfo> GetCheckLists(GameState gameState)
        {
            var builder = new CheckListsBuilder();
            return builder.CheckLists;
        }

        public override Task Execute(GameTaskContext context)
        {
            throw new NotImplementedException();
        }

        public override GameTaskValidationResult Validate(GameTaskContext context)
        {
            return new GameTaskValidationResult();
        }
    }
}
