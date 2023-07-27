using Shared_Resources.Constants.CheckListsBuilding;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared_Resources.GameTasks.Implementations_Unity;

public class CookStewBase : GameTaskBase
{
    public override GameTaskCodes Code => GameTaskCodes.CookStew;

    public override GameTaskCategory Category => GameTaskCategory.Room;

    public override bool HasRequiredConditions(GameState gameState)
    {
        bool isInValidRoom = gameState.LocalPlayerRoom.Name == nameof(RoomsTemplate.Galley);
        return isInValidRoom;
    }

    public override List<PromptInfo> GetCheckLists(GameState gameState)
    {
        var builder = new CheckListsBuilder();
        return builder.CheckLists;
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
