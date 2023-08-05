using Shared_Resources.GameTasks.CheckListsBuilding;
using Shared_Resources.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared_Resources.GameTasks;

public abstract class GameTaskBase : IGameTask
{
    public abstract GameTaskCodes Code { get; }
    public abstract GameTaskCategory Category { get; }
    public abstract bool HasRequiredConditions(GameState gameState);
    public abstract Task Execute(GameTaskContext context);
    public abstract List<PromptInfo> GetCheckLists(GameState gameState);
    public abstract GameTaskValidationResult Validate(GameTaskContext context);
}
