using Shared_Resources.Entities;
using Shared_Resources.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Shared_Resources.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<string> RoleNamesAsString => RoleNames.Select(x => x.ToString()).ToList();
    public List<RoleName> RoleNames { get; set; } = new List<RoleName>(); // fluent api
    public List<Player> Players { get; set; } = [];
    public List<LobbyDto> QueuedLobbies { get; set; } = [];
    public List<GameDto> ActiveGames { get; set; } = [];
    public List<Lobby> AvailableLobbies { get; set; } = [];
}
