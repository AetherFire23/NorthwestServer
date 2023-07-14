using Shared_Resources.Constants;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared_Resources.GameTasks.Implementations_Unity;

public class RepairCannon : GameTaskBase
{
    public override GameTaskCodes Code => GameTaskCodes.RepairCannon;

    public override GameTaskCategory Category => GameTaskCategory.Player;



    public override bool HasRequiredConditions(GameState gameState)
    {
        List<string> validRoomsNames = new List<string>()
        {
            nameof(RoomsTemplate.QuarterDeck),
            nameof(RoomsTemplate.Forecastle),
        };
        bool isInValidRoom = validRoomsNames.Contains(gameState.LocalPlayerRoom.Name);
        return isInValidRoom;
    }

    public override List<PromptInfo> GetCheckLists(GameState gameState)
    {
        var builder = new CheckListsBuilder();
        return builder.CheckLists;
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
