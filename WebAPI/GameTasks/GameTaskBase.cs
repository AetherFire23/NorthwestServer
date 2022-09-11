namespace WebAPI.GameTasks
{
    public abstract class GameTaskBase : IGameTask
    {
        public abstract GameTaskValidationResult Validate(GameTaskContext context);
        public abstract void Execute(GameTaskContext context);

        protected void SomeSharedLogic()
        {

        }
    }
}