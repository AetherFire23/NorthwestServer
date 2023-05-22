﻿using Shared_Resources.Constants;
using Shared_Resources.Enums;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared_Resources.GameTasks.Implementations_Unity
{
    public class LowerSail : GameTaskBase
    {
        public override GameTaskCodes Code => GameTaskCodes.LowerSail;

        public override GameTaskProvider Provider => GameTaskProvider.Room;

        public override bool Requires(GameState gameState)
        {
            List<string> validRoomsNames = new List<string>()
            {
                nameof(RoomTemplate2.QuarterDeck),
                nameof(RoomTemplate2.Forecastle),
                nameof(RoomTemplate2.MainDeck),
            };
            bool isInValidRoom = validRoomsNames.Contains(gameState.Room.Name);
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