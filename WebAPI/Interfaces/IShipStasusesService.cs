namespace WebAPI.Interfaces
{
    public interface IShipStasusesService
    {
        Task InitializeShipStatusesAndResources(Guid gameId);
    }
}