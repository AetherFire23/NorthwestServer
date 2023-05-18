using Shared_Resources.Entities;
using Shared_Resources.Enums;

namespace WebAPI.Strategies
{
    [RoleStrategy(RoleType.Commander)]
    public class CommanderRoleStrategy : IRoleInitializationStrategy
    {
        public async Task InitializePlayerFromRoleAsync(Player player)
        {
            Console.WriteLine("CommanderStrat!");
        }
    }
}
