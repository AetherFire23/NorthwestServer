using WebAPI.GameState_Management;

namespace WebAPI.GameTasks
{
    [GameTask(GameTaskCode.Cook)]
    public class CookTask : GameTaskBase
    {
        public override GameTaskValidationResult Validate(GameTaskContext context)
        {
            if (context.Player.Z > 10)
            {
                return new GameTaskValidationResult($"The current Z is {context.Player.Z} and 10 is needed.");
            }


            return new GameTaskValidationResult();
        }

        public override void Execute(GameTaskContext context)
        {
            SomeSharedLogic();
        }
    }
}