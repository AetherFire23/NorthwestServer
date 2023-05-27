using Shared_Resources.DTOs;
using Shared_Resources.Models;
using System.Collections.Generic;

namespace Shared_Resources.GameTasks
{
    public class GameTaskContext
    {
        public GameState GameState { get; set; }
        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();// devrait etre apreil partout donc utiliser : object Params
        public PlayerDTO Player => GameState.PlayerDTO;
    }
}
