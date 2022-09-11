namespace WebAPI.GameTasks
{
    [GameTask(GameTaskCode.Kill)]
    public class KillTask : GameTaskBase
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