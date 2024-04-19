using Northwest.DTOs;
using Northwest.Models;

namespace Northwest.GameTasks;

public class GameTaskExecutionContext
{
    public GameState GameState { get; set; }
    public List<List<GameTaskTargetInfo>> Parameters { get; set; } = new();
    public PlayerDto Player => GameState.PlayerDto;
    public Guid GameId => GameState.GameId;
}
