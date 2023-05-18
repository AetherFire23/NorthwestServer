using Shared_Resources.Entities;
using Shared_Resources.Enums;

namespace WebAPI.Strategies
{
    [RoleStrategy(RoleType.Medic)]
    public class MedicRoleStrategyService : IRoleInitializationStrategy
    {
        private readonly PlayerContext _playerContext;

        public MedicRoleStrategyService(PlayerContext playerContext)
        {
            _playerContext = playerContext;
        }

        public async Task InitializePlayerFromRoleAsync(Player player)
        {
            int i = 0;
        }
    }
}
