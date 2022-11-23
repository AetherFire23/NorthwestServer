using WebAPI.DTOs;
using WebAPI.GameState_Management;

namespace WebAPI.GameTasks
{
    public class GameTaskContext
    {
        public GameState GameState { get; set; }
        public Dictionary<string, string> Parameters { get; set; } // devrait etre apreil partout donc utiliser : object Params
        public PlayerDTO Player => GameState.PlayerDTO;
    }
}