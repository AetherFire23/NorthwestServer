using Northwest.Domain.Interfaces;
using Northwest.Domain.Models;
using Northwest.Domain.Repositories;
using Northwest.Persistence;
using Northwest.Persistence.Entities;
using Northwest.Persistence.Enums;

namespace Northwest.Domain.Strategies;

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
