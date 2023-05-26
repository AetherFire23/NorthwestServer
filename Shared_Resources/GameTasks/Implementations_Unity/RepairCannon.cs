using Shared_Resources.Constants;
using Shared_Resources.Enums;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared_Resources.GameTasks.Implementations_Unity
{
    public class RepairCannon : GameTaskBase
    {
        public override GameTaskCodes Code => GameTaskCodes.RepairCannon;

        public override GameTaskProvider Provider => GameTaskProvider.Player;



        public override bool Requires(GameState gameState)
        {
            List<string> validRoomsNames = new List<string>()
            {
                nameof(RoomsTemplate.QuarterDeck),
                nameof(RoomsTemplate.Forecastle),
            };
            bool isInValidRoom = validRoomsNames.Contains(gameState.Room.Name);
            return isInValidRoom;
        }

        public override CheckListsBuilder GetValidTargetPrompts(GameState gameState)
        {
            var targets = new CheckListsBuilder();
            return targets;
        }
        public override GameTaskValidationResult Validate(GameTaskContext context)
        {
            return new GameTaskValidationResult();
        }
        public override Task Execute(GameTaskContext context)
        {
            throw new NotImplementedException();
        }
    }
}
