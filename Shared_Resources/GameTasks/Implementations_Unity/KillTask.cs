using Shared_Resources.Constants;
using Shared_Resources.Enums;
using Shared_Resources.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shared_Resources.GameTasks.Implementations_Unity
{
    public class KillTask : GameTaskBase
    {
        public override GameTaskCodes Code => GameTaskCodes.Kill;

        public override GameTaskProvider Provider => GameTaskProvider.Player;

        public override bool CanShow(GameState gameState)
        {
            return gameState.PlayerDTO.Items.Exists(item => item.ItemType.Equals(ItemType.Wrench));
        }

        public override Task Execute(GameTaskContext context)
        {
            throw new NotImplementedException();
        }

        public override CheckListsBuilder GetValidTargetPrompts(GameState gameState)
        {
            var targets = new CheckListsBuilder();

            var rooms = gameState.Rooms;
            targets.CreateCheckListPrompt(rooms).SetExactAmount(1);

            return targets;
        }

        public override GameTaskValidationResult Validate(GameTaskContext context)
        {
            return new GameTaskValidationResult();
        }
    }
}
