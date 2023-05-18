using Shared_Resources.Entities;

namespace WebAPI.Strategies
{
    public interface IRoleInitializationStrategy
    {
        Task InitializePlayerFromRoleAsync(Player player);
    }
}
