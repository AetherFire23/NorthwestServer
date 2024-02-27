using WebAPI.Entities;
using WebAPI.Enums;
using WebAPI.Interfaces;
using WebAPI.Models;
using WebAPI.Repositories;

namespace WebAPI.Strategies;

[RoleStrategy(RoleType.Medic)]
public class MedicRoleStrategyService : IRoleInitializationStrategy
{
    private readonly PlayerContext _playerContext;
    private readonly RoomRepository _roomRepository;
    public MedicRoleStrategyService(PlayerContext playerContext, RoomRepository roomRepository)
    {
        _playerContext = playerContext;
        _roomRepository = roomRepository;
    }

    public async Task InitializePlayerFromRoleAsync(Player player)
    {
        Room room = await _roomRepository.GetRoomFromName(player.GameId, nameof(RoomsTemplate.CrowsNest));
        player.CurrentGameRoomId = room.Id;

        Log log = new Log()
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

        Log logPublic = new Log()
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

        LogAccessPermissions permission = new LogAccessPermissions()
        {
            Id = Guid.NewGuid(),
            LogId = log.Id,
            PlayerId = player.Id,
        };

        _ = _playerContext.LogAccessPermission.Add(permission); // seeding some log for specific players
        _ = _playerContext.Logs.Add(log); // seeding some log for specific players
        _ = _playerContext.Logs.Add(logPublic); // seeding some log for specific players

        _ = await _playerContext.SaveChangesAsync();
    }

    public async Task TickPlayerFromRoleAsync(Player player)
    {

    }
}
