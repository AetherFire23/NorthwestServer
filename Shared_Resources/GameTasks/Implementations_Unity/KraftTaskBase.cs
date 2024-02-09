using Shared_Resources.GameTasks.CheckListsBuilding;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared_Resources.GameTasks.Implementations_Unity;

public class KraftTaskBase : GameTaskBase
{
    public override GameTaskCodes Code => GameTaskCodes.CraftTask;
    public override GameTaskCategory Category => GameTaskCategory.Room;

    public override bool HasRequiredConditions(GameState gameState)
    {
        // Pas de liste juste if, brackets et ||
        _ = gameState.LocalPlayerRoom.Name.Equals(nameof(RoomsTemplate.CaptainsQuarters));
        bool isBelowMaximumItemCount = gameState.PlayerDTO.Items.Count < 2;
        _ = gameState.LocalPlayerRoom.Name.Equals(nameof(RoomsTemplate.CrowsNest));
        return isBelowMaximumItemCount;
    }

    // return list of 
    // rename PromptInfo to checklist
    public override List<PromptInfo> GetCheckLists(GameState gameState)
    {
        CheckListsBuilder builder = new CheckListsBuilder();

        List<DTOs.RoomDTO> rooms = gameState.Rooms.Where(x => x.IsLandmass).ToList();
        _ = builder.CreateCheckListPrompt(rooms, "Changed it!")
            .SetMinimumTargetCount(2)
            .SetMaximumTargetCount(5);
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
