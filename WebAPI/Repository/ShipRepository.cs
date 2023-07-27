﻿using Microsoft.EntityFrameworkCore;
using Shared_Resources.Entities;
using WebAPI.Interfaces;

namespace WebAPI.Repository;

public class ShipRepository : IShipRepository
{
    private readonly PlayerContext _playerContext;
    public ShipRepository(PlayerContext playerContext)
    {
        _playerContext = playerContext;
    }

    public async Task<ShipState> GetShipStateAsync(Guid gameId)
    {
        ShipState shipState = await _playerContext.ShipStates.FirstAsync(s => s.GameId == gameId);
        return shipState;
    }
}
