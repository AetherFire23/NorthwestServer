using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using System;
using System.Collections.Generic;

namespace Shared_Resources.Models;

public class NewGameInfo
{
    public List<UserDto> Users { get; set; }
    public List<UserGamePreparation> UserGamePreparation { get; set; }
    public Game Game { get; set; } // created later }
    public Guid GameId => Game.Id;
}
