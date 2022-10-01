namespace WebAPI.GameTasks
{
    public class GameTaskValidationResult
    {
        public GameTaskValidationResult()
        {

        }

        public GameTaskValidationResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; } = string.Empty;
        public bool IsValid => string.IsNullOrWhiteSpace(ErrorMessage);
    }
}