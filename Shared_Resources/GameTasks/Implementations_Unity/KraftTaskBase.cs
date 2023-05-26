using Shared_Resources.Constants;
using Shared_Resources.Enums;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared_Resources.GameTasks.Implementations_Unity
{
    public class KraftTaskBase : GameTaskBase
    {
        public override GameTaskCodes Code => GameTaskCodes.CraftTask;

        public override GameTaskProvider Provider => GameTaskProvider.Room;

        public override bool Requires(GameState gameState)
        {
            var conditions = new List<bool>();
            conditions.Add(gameState.Room.Name.Equals(nameof(RoomsTemplate.CaptainsQuarters)));
            conditions.Add(gameState.PlayerDTO.Items.Count < 2);
            return conditions.All(c => c is true);
        }

        public override CheckListsBuilder GetValidTargetPrompts(GameState gameState)
        {
            var checkList = new CheckListsBuilder();

            var rooms = gameState.Rooms.Where(x => x.IsLandmass).ToList();
            checkList.CreateCheckListPrompt(rooms, "Changed it!")
                .SetExactAmount(1);
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
