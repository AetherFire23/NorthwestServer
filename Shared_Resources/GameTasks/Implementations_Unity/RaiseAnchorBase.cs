using Shared_Resources.GameTasks.CheckListsBuilding;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared_Resources.GameTasks.Implementations_Unity;

public class RaiseAnchorBase : GameTaskBase
{
    public override GameTaskCodes Code => GameTaskCodes.RaiseAnchor;

    public override GameTaskCategory Category => GameTaskCategory.Room;

    public override bool HasRequiredConditions(GameState gameState)
    {
        bool isInValidRoom = gameState.LocalPlayerRoom.Name == nameof(RoomsTemplate.Forecastle);
        return isInValidRoom;
    }

    public override List<PromptInfo> GetCheckLists(GameState gameState)
    {
        CheckListsBuilder checkList = new CheckListsBuilder();
        return checkList.CheckLists;
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
