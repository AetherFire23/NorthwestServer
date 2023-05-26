﻿using Shared_Resources.Constants;
using Shared_Resources.Enums;
using Shared_Resources.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Shared_Resources.GameTasks.Implementations_Unity
{
    public class CookFishBase : GameTaskBase
    {
        public override GameTaskCodes Code => GameTaskCodes.CookFish;

        public override GameTaskProvider Provider => GameTaskProvider.Room;

        public override bool Requires(GameState gameState)
        {
            bool isInValidRoom = gameState.Room.Name == nameof(RoomsTemplate.Galley);
            return isInValidRoom;
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
