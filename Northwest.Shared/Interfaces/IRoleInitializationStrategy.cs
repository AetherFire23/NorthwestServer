using Northwest.Shared.Entities;

namespace WebAPI.Interfaces;

public interface IRoleInitializationStrategy
{
    Task InitializePlayerFromRoleAsync(Player player);
    Task TickPlayerFromRoleAsync(Player player);
}
