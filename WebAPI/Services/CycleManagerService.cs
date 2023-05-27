using Shared_Resources.Entities;
using Shared_Resources.Enums;
using WebAPI.Game_Actions;
using WebAPI.Interfaces;
using WebAPI.Strategies;
using WebAPI.UniversalSkills;

namespace WebAPI.Services
{
    public class CycleManagerService : ICycleManagerService
    {
        public static int TimeBetweenTicksInSeconds = 8;
        private readonly PlayerContext _playerContext;
        private readonly IGameRepository _gameRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IServiceProvider _serviceProvider;
        public CycleManagerService(PlayerContext playerContext,
            IGameRepository gameRepository,
            IPlayerRepository playerRepository,
            IServiceProvider serviceProvider)
        {
            _playerContext = playerContext;
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
            _serviceProvider = serviceProvider;
        }

        public async Task TickGame(Guid gameId)
        {
            var game = await _gameRepository.GetGame(gameId);

            var playersInGame = await _playerRepository.GetPlayersInGameAsync(gameId);
            await TickPlayerFromRoles(playersInGame);

            game.NextTick = DateTime.UtcNow.AddSeconds(TimeBetweenTicksInSeconds);

            await _playerContext.SaveChangesAsync();
            Console.WriteLine("Game Has ticked");
        }

        public async Task TickUniversalSKills(Guid gameId) // pretty much ready-togo
        {
            var universalSkillTypes = SkillStrategyMapper.GetAllUniversalSkillTypes();
            var universalSkills = universalSkillTypes.Select(x => _serviceProvider.GetService(x) as IUniversalSkill).ToList();

            var players = await _playerRepository.GetPlayersInGameAsync(gameId);

            foreach (var player in players)
            {
                foreach (var universalSkill in universalSkills)
                {
                    var canTickSkill = await universalSkill.CanApplySkillEffect(player);
                    if (!canTickSkill) continue;

                    await universalSkill.ApplyTickEffect(player);
                }
            }
        }

        public async Task TickPlayerFromRoles(List<Player> playersInGame)
        {
            foreach (Player p in playersInGame)
            {
                var roleManagerType = RoleStrategyMapper.GetStrategyTypeByRole(p.Profession);
                var roleService = _serviceProvider.GetService(roleManagerType) as IRoleInitializationStrategy;
                await roleService.TickPlayerFromRoleAsync(p);
            }
        }
    }
}
