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
    public class FireCannon : GameTaskBase
    {
        // could add reflection check to see if I have forgotten to set a taskCode/ provider
        public override GameTaskCategory Category { get; } = GameTaskCategory.Room;
        public override GameTaskCodes Code { get; } = GameTaskCodes.FireCannon;

        public override bool HasRequiredConditions(GameState gameState)
        {
            List<string> validRoomsNames = new List<string>()
            {
                nameof(RoomsTemplate.QuarterDeck),
                nameof(RoomsTemplate.Forecastle),
            };
            bool isInValidRoom = validRoomsNames.Contains(gameState.Room.Name);
            return isInValidRoom;
        }

        public override GameTaskValidationResult Validate(GameTaskContext context)
        {
            return new GameTaskValidationResult();
        }

        public override Task Execute(GameTaskContext context)
        {
            throw new NotImplementedException();
        }

        // renvoie un dictionnaire parce que je me force a sorter les types 
        public override List<PromptInfo> GetCheckLists(GameState gameState)
        {
            return new CheckListsBuilder().CheckLists;
        }
    }
}
