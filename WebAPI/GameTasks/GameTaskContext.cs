using WebAPI.GameState_Management;

namespace WebAPI.GameTasks
{
    public class GameTaskContext
    {
        public GameState GameState { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public Player Player => GameState.Player;
    }
}