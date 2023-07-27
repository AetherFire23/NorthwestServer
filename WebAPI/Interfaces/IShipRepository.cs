using Shared_Resources.Entities;

namespace WebAPI.Interfaces;

public interface IShipRepository
{
    Task<ShipState> GetShipStateAsync(Guid gameId);
}