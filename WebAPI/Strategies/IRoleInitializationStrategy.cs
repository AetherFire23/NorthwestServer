using Shared_Resources.Entities;

namespace WebAPI.Strategies
{
    public interface IRoleInitializationStrategy
    {
        Task InitializePlayerFromRole(PlayerContext context, Player player);
    }
}
