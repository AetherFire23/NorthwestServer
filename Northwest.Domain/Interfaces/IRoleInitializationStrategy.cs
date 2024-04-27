using Northwest.Persistence.Entities;

namespace Northwest.Domain.Interfaces;

public interface IRoleInitializationStrategy
{
    Task InitializePlayerFromRoleAsync(Player player);
    Task TickPlayerFromRoleAsync(Player player);
}
