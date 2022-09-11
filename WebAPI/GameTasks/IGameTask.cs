namespace WebAPI.GameTasks
{
    public interface IGameTask
    {
        /// <summary>
        /// Validates if a player can execute a task or not
        /// </summary>
        /// <param name="player">The player</param>
        /// <returns>An error code, if any</returns>
        GameTaskValidationResult Validate(GameTaskContext context);

        /// <summary>
        /// Executes a game task
        /// </summary>
        /// <param name="player">The player</param>
        void Execute(GameTaskContext context);
    }
}