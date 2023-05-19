using Shared_Resources.DTOs;
using Shared_Resources.Entities;

namespace WebAPI.Interfaces
{
    public interface IShipStatusesRepository
    {
        Task<ShipState> GetShipStateAsync(Guid gameId);
    }
}