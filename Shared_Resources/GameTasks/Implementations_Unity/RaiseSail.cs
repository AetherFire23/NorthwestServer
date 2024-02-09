using Shared_Resources.Enums;
using Shared_Resources.GameTasks.CheckListsBuilding;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared_Resources.GameTasks.Implementations_Unity;

public class RaiseSail : GameTaskBase
{
    public override GameTaskCodes Code => GameTaskCodes.RaiseSail;

    public override GameTaskCategory Category => GameTaskCategory.Sailing;

    public override bool HasRequiredConditions(GameState gameState)
    {
        bool hasValidItem = gameState.PlayerDTO.Items.Exists(x => x.ItemType == ItemType.Wrench);

        Entities.Item firstItem = gameState.PlayerDTO.Items.First();
        // PEDRO GIGACHAD
        if (firstItem.ItemType is ItemType.Wrench and not ItemType.Hose)
        {

        }

        List<string> validRoomsNames = new List<string>
        {
            nameof(RoomsTemplate.QuarterDeck),
            nameof(RoomsTemplate.Forecastle),
            nameof(RoomsTemplate.MainDeck),
        };
        bool isInValidRoom = validRoomsNames.Contains(gameState.LocalPlayerRoom.Name);
        return isInValidRoom && hasValidItem;
    }

    public override List<PromptInfo> GetCheckLists(GameState gameState)
    {
        CheckListsBuilder builder = new CheckListsBuilder();
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
