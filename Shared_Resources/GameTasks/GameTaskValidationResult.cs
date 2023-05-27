namespace Shared_Resources.GameTasks
{

    public class GameTaskValidationResult
    {
        public string ErrorMessage { get; } = string.Empty;
        public bool IsValid => string.IsNullOrWhiteSpace(ErrorMessage);

        /// <summary>
        /// Constructorless means success
        /// </summary>
        public GameTaskValidationResult()
        {
            
        }
        public GameTaskValidationResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

    }
}
