using Shared_Resources.Entities;
using WebAPI.Interfaces;
using WebAPI.Repositories;
using WebAPI.Strategies;
using WebAPI.UniversalSkills;
namespace WebAPI.Services;

public class CycleManagerService
{
    private readonly PlayerContext _playerContext;
    private readonly GameRepository _gameRepository;
    private readonly PlayerRepository _playerRepository;
    private readonly IServiceProvider _serviceProvider;
    public CycleManagerService(PlayerContext playerContext,
        GameRepository gameRepository,
        PlayerRepository playerRepository,
        IServiceProvider serviceProvider)
    {
        _playerContext = playerContext;
        _gameRepository = gameRepository;
        _playerRepository = playerRepository;
        _serviceProvider = serviceProvider;
    }

    public async Task TickGame(Guid gameId)
    {
        Game game = await _gameRepository.GetGameById(gameId);

        List<Player> playersInGame = await _playerRepository.GetPlayersInGameAsync(gameId);
        await TickPlayerRoles(playersInGame);

        game.NextTick = Game.CalculateNextTick();

        _ = await _playerContext.SaveChangesAsync();
        Console.WriteLine("Game Has ticked");
    }

    public async Task TickUniversalSKills(Guid gameId) // pretty much ready-togo
    {
        List<Type> universalSkillTypes = SkillStrategyMapper.GetAllUniversalSkillTypes();
        List<ITickedSkills?> universalSkills = universalSkillTypes.Select(x => _serviceProvider.GetService(x) as ITickedSkills).ToList();

        List<Player> players = await _playerRepository.GetPlayersInGameAsync(gameId);

        foreach (Player player in players)
        {
            foreach (ITickedSkills? universalSkill in universalSkills)
            {
                bool canTickSkill = await universalSkill.CanApplySkillEffect(player);
                if (!canTickSkill) continue;

                await universalSkill.ApplyTickEffect(player);
            }
        }
    }

    public async Task TickPlayerRoles(List<Player> playersInGame)
    {
        foreach (Player p in playersInGame)
        {
            Type roleManagerType = RoleStrategyMapper.GetStrategyTypeByRole(p.Profession);
            IRoleInitializationStrategy? roleService = _serviceProvider.GetService(roleManagerType) as IRoleInitializationStrategy;
            await roleService.TickPlayerFromRoleAsync(p);
        }
    }
}
