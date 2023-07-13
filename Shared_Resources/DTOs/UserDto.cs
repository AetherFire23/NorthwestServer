using Newtonsoft.Json;
using Shared_Resources.Entities;
using Shared_Resources.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared_Resources.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // may have to rename to just Role[]
        public List<string> RoleNamesAsString => RoleNames.Select(x => x.ToString()).ToList();

        public List<RoleName> RoleNames { get; set; } = new List<RoleName>(); // fluent api

        public List<Player> Players { get; set; } = new List<Player>();
        public List<LobbyDto> QueuedLobbies { get; set; } = new List<LobbyDto>();
        public List<GameDto> ActiveGamesForUser { get; set; } = new List<GameDto>();
    }
}
