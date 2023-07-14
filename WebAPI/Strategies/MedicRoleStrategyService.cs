using Shared_Resources.Entities;
using Shared_Resources.Enums;
using Shared_Resources.Models;
using WebAPI.Interfaces;

namespace WebAPI.Strategies;

[RoleStrategy(RoleType.Medic)]
public class MedicRoleStrategyService : IRoleInitializationStrategy
{
    private readonly PlayerContext _playerContext;
    private readonly IRoomRepository _roomRepository;
    private readonly IPlayerRepository _playerRepository;
    public MedicRoleStrategyService(PlayerContext playerContext,
        IRoomRepository roomRepository,
        IPlayerRepository playerRepository)
    {
        _playerContext = playerContext;
        _roomRepository = roomRepository;
        _playerRepository = playerRepository;
    }

    public async Task InitializePlayerFromRoleAsync(Player player)
    {
        var room = await _roomRepository.GetRoomFromName(player.GameId, nameof(RoomsTemplate.CrowsNest));
        player.CurrentGameRoomId = room.Id;

        var log = new Log()
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            CreatedBy = "master",
            EventText = "MyEvent",
            GameId = player.GameId,
            IsPublic = false,
            RoomId = player.CurrentGameRoomId,
            TriggeringPlayerId = player.Id,
        };

        var logPublic = new Log()
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            CreatedBy = "master",
            EventText = "Master Public",
            GameId = player.GameId,
            IsPublic = true,
            RoomId = player.CurrentGameRoomId,
            TriggeringPlayerId = player.Id,
        };

        var permission = new LogAccessPermissions()
        {
            Id = Guid.NewGuid(),
            LogId = log.Id,
            PlayerId = player.Id,
        };

        _playerContext.LogAccessPermission.Add(permission); // seeding some log for specific players
        _playerContext.Logs.Add(log); // seeding some log for specific players
        _playerContext.Logs.Add(logPublic); // seeding some log for specific players

        await _playerContext.SaveChangesAsync();
    }

    public async Task TickPlayerFromRoleAsync(Player player)
    {

    }
}
