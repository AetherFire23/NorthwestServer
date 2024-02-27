using WebAPI.DTOs;
using WebAPI.Models;

namespace WebAPI.GameTasks;

public class GameTaskExecutionContext
{
    public GameState GameState { get; set; }
    public TaskParameters Parameters { get; set; } = new TaskParameters();
    public PlayerDto Player => GameState.PlayerDTO;
    public Guid GameId => GameState.GameId;
}
