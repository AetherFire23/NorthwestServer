using Shared_Resources.Constants;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Shared_Resources.GameTasks.Implementations_Unity
{
    public class ChargeCannon : GameTaskBase
    {
        public override GameTaskCodes Code => GameTaskCodes.ChargeCannon;

        public override GameTaskProvider Provider => GameTaskProvider.Room;

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

        public override Task Execute(GameTaskContext context)
        {
            throw new NotImplementedException();
        }

        public override CheckListsBuilder GetValidTargetPrompts(GameState gameState)
        {
            return new CheckListsBuilder();
        }

        public override GameTaskValidationResult Validate(GameTaskContext context)
        {
            return new GameTaskValidationResult();
        }
    }
}
