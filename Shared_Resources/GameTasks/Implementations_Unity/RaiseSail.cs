using Shared_Resources.Constants;
using Shared_Resources.Enums;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared_Resources.GameTasks.Implementations_Unity
{
    public class RaiseSail : GameTaskBase
    {
        public override GameTaskCodes Code => GameTaskCodes.RaiseSail;

        public override GameTaskProvider Provider => GameTaskProvider.Sailing;

        public override bool Requires(GameState gameState)
        {
            bool hasValidItem = gameState.PlayerDTO.Items.Exists(x => x.ItemType == ItemType.Wrench);

            List<string> validRoomsNames = new List<string>()
            {
                nameof(RoomsTemplate.QuarterDeck),
                nameof(RoomsTemplate.Forecastle),
                nameof(RoomsTemplate.MainDeck),
            };
            bool isInValidRoom = validRoomsNames.Contains(gameState.Room.Name);
            return isInValidRoom && hasValidItem;
        }

        public override CheckListsBuilder GetValidTargetPrompts(GameState gameState)
        {
            var checkList = new CheckListsBuilder();
            return checkList;
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
