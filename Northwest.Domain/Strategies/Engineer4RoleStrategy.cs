using Northwest.Domain.Interfaces;
using Northwest.Domain.Models;
using Northwest.Domain.Repositories;
using Northwest.Persistence;
using Northwest.Persistence.Entities;
using Northwest.Persistence.Enums;

namespace Northwest.Domain.Strategies;

[RoleStrategy(RoleType.Engineer4)]
public class Engineer4RoleStrategy(PlayerContext playerContext, RoomRepository roomRepository) : IRoleInitializationStrategy
{
    public async Task InitializePlayerFromRoleAsync(Player player) // not added to Db yet
    {
        var room = await roomRepository.GetRoomFromName(player.GameId, nameof(RoomsTemplate.MiddleCorridor));
        player.CurrentGameRoomId = room.Id;

        var item = new Item()
        {
            Id = Guid.NewGuid(),
            ItemType = ItemType.Wrench,
            OwnerId = player.Id,
        };

        await playerContext.Items.AddAsync(item);
        await playerContext.SaveChangesAsync();
    }

    public async Task TickPlayer(Guid playerId)
    {
        // Ticker pour chaque skill ? 
        await Task.CompletedTask;
    }

    public async Task TickPlayerFromRoleAsync(Player player)
    {
        await Task.CompletedTask;
    }
}
