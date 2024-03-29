﻿using Shared_Resources.DTOs;
using System.Collections.Generic;

namespace Shared_Resources.Models.SSE.TransferData;

public class RefreshLobbiesAndGamesTransfer
{
    public List<LobbyDto> QueuedLobbies { get; set; } = new List<LobbyDto>();
    public List<GameDto> ActiveGamesForUser { get; set; } = new List<GameDto>();
}
