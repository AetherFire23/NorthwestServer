using Shared_Resources.DTOs;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared_Resources.GameTasks
{
    public class GameTaskContext
    {
        public GameState GameState { get; set; }
        public Dictionary<string, string> Parameters { get; set; } // devrait etre apreil partout donc utiliser : object Params
        public PlayerDTO Player => GameState.PlayerDTO;
    }
}
