using Shared_Resources.Constants.CheckListsBuilding;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared_Resources.GameTasks.Implementations_Unity;

public class ChargeCannon : GameTaskBase
{
    public override GameTaskCodes Code => GameTaskCodes.ChargeCannon;

    public override GameTaskCategory Category => GameTaskCategory.Room;

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

    public override Task Execute(GameTaskContext context)
    {
        throw new NotImplementedException();
    }

    public override List<PromptInfo> GetCheckLists(GameState gameState)
    {
        return new CheckListsBuilder().CheckLists;
    }

    public override GameTaskValidationResult Validate(GameTaskContext context)
    {
        return new GameTaskValidationResult();
    }
}
