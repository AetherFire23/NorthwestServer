using WebAPI.GameState_Management;

namespace WebAPI.GameTasks
{
    [GameTask(GameTaskCode.Repair)]
    public class RepairTask : GameTaskBase
    {
        public override GameTaskValidationResult Validate(GameTaskContext context)
        {
            return new GameTaskValidationResult();
        }

        public override void Execute(GameTaskContext context)
        {
            SomeSharedLogic();
        }
    }
}