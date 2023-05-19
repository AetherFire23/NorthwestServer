using Microsoft.EntityFrameworkCore;
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using Shared_Resources.Enums;
using System.Security.AccessControl;
using WebAPI.Interfaces;

namespace WebAPI.Repository
{
    public class ShipStatusesRepository : IShipStatusesRepository
    {
        private readonly PlayerContext _playerContext;
        public ShipStatusesRepository(PlayerContext playerContext)
        {
            _playerContext = playerContext;
        }

        public async Task<ShipState> GetShipStateAsync(Guid gameId)
        {
            ShipState shipState = await _playerContext.ShipStates.FirstAsync(s => s.GameId == gameId);
            return shipState;
        }
    }
}
