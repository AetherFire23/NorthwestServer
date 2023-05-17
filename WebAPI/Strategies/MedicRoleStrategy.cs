using Shared_Resources.Entities;
using Shared_Resources.Enums;

namespace WebAPI.Strategies
{
    [RoleStrategy(RoleType.Medic)]
    public class MedicRoleStrategy : IRoleInitializationStrategy
    {
        public Task InitializePlayerFromRole(PlayerContext context, Player player)
        {
            throw new NotImplementedException();
        }
    }
}
