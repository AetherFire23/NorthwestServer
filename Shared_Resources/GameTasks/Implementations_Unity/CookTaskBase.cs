using Shared_Resources.Constants;
using Shared_Resources.Entities;
using Shared_Resources.Enums;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared_Resources.GameTasks.Implementations_Unity
{
    public class CookTaskBase : GameTaskBase
    {
        // could add reflection check to see if I have forgotten to set a taskCode/ provider
        public override GameTaskProvider Provider { get; } = GameTaskProvider.Room;
        public override GameTaskCodes Code { get; } = GameTaskCodes.Cook;

        public override bool Requires(GameState gameState)
        {
            string currentRoomName = gameState.Room.Name;
            bool isPlayerInCorrectRoom = currentRoomName.Equals(nameof(RoomTemplate2.Galley)); //|| currentRoomName.Equals("EntryHall");
            return isPlayerInCorrectRoom;
        }

        public override GameTaskValidationResult Validate(GameTaskContext context)
        {
            if (context.GameState.Room.Name != nameof(RoomTemplate2.Galley))
            {
                return new GameTaskValidationResult("");
            }

            return new GameTaskValidationResult();
        }

        public override Task Execute(GameTaskContext context)
        {
            throw new NotImplementedException();
        }

        // renvoie un dictionnaire parce que je me force a sorter les types 
        public override CheckListsBuilder GetValidTargetPrompts(GameState gameState)
        {
            var targetPrompts = new CheckListsBuilder();
            List<DTOs.RoomDTO> rooms = gameState.Rooms;
            List<Player> players = gameState.Players;

            targetPrompts.CreateCheckListPrompt(rooms)
                .SetExactAmount(3);

            targetPrompts.CreateCheckListPrompt(players)
                .SetMinimumTargetCount(1)
                .SetMaximumTargetCount(1);

            var stations = gameState.Stations;
            targetPrompts.CreateCheckListPrompt(stations)
                .SetExactAmount(1);

            return targetPrompts;
        }
    }
}
