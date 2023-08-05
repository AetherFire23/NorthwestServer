using Shared_Resources.DTOs;
using Shared_Resources.Models;
using System;

namespace Shared_Resources.GameTasks;

public class GameTaskContext
{
    public GameState GameState { get; set; }
    public TaskParameters Parameters { get; set; } = new TaskParameters();
    public PlayerDto Player => GameState.PlayerDTO;
    public Guid GameId => GameState.GameId;
}
