using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared_Resources.GameTasks.Implementations_Unity
{
    public class CookTaskValidate : IGameTask 
    {
        public GameTaskProvider Provider { get; set; } = GameTaskProvider.Room;
        bool IGameTask.RequiresPlayerTarget { get; set; } = false;

        public bool CanShow(GameState gameState)
        {
            bool correctRoom = gameState.Room.Name.Equals("Kitchen1");
            return correctRoom;
        }

        public GameTaskValidationResult Validate(GameTaskContext context)
        {
            if (context.GameState.Room.Name != nameof(LevelTemplate.Kitchen1))
            {
                return new GameTaskValidationResult("");
            }

            return new GameTaskValidationResult();
        }

        public virtual void Execute(GameTaskContext context)
        {
            throw new NotImplementedException("Cannot execute a task inside Unity.");
        }
    }
}
