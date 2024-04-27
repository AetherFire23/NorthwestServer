using Microsoft.EntityFrameworkCore;
using Northwest.Persistence;
using Northwest.Persistence.Entities;
namespace Northwest.Domain.Repositories;

public class ShipRepository
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
