using Shared_Resources.Constants;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Shared_Resources.GameTasks.Implementations_Unity
{
    public class CleanTaskBase : GameTaskBase
    {
        public override GameTaskCodes Code => GameTaskCodes.Clean;

        public override GameTaskProvider Provider => GameTaskProvider.Player;

        public override bool Requires(GameState gameState)
        {
            return true;
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
