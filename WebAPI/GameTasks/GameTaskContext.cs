using WebAPI.GameState_Management;
using WebAPI.Models.DTOs;

namespace WebAPI.GameTasks
{
    public class GameTaskContext
    {
        public GameState GameState { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public PlayerDTO Player => GameState.PlayerDTO;
    }
}