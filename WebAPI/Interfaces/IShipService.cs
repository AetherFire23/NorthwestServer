namespace WebAPI.Interfaces;

public interface IShipService
{
    Task InitializeShipStatusesAndResources(Guid gameId);
}