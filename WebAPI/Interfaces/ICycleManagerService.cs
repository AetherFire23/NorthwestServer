namespace WebAPI.Interfaces;

public interface ICycleManagerService
{
    Task TickGame(Guid gameId);
}
