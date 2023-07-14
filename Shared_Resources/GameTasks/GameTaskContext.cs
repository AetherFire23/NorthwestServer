﻿using Shared_Resources.DTOs;
using Shared_Resources.Models;
using Shared_Resources.Scratches;

namespace Shared_Resources.GameTasks;

public class GameTaskContext
{
    public GameState GameState { get; set; }
    public TaskParameters Parameters { get; set; } = new TaskParameters();
    public PlayerDTO Player => GameState.PlayerDTO;
}
