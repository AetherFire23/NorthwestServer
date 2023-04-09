using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared_Resources.GameTasks
{
    public class GameTaskValidationResult
    {
        public string ErrorMessage { get; } = string.Empty;
        public bool IsValid => string.IsNullOrWhiteSpace(ErrorMessage);

        public GameTaskValidationResult()
        {
            
        }
        public GameTaskValidationResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

    }
}
