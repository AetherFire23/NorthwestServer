﻿using Northwest.Domain.Interfaces;
using Northwest.Domain.Models;
using Northwest.Domain.Repositories;
using Northwest.Persistence;
using Northwest.Persistence.Entities;
using Northwest.Persistence.Enums;

namespace Northwest.Domain.Strategies;

[RoleStrategy(RoleType.Cook)]
public class CookRoleStrategy : IRoleInitializationStrategy
{
    private readonly PlayerContext _playerContext;
    private readonly RoomRepository _roomRepository;
    private readonly PlayerRepository _playerRepository;
    public CookRoleStrategy(PlayerContext playerContext, RoomRepository roomRepository, PlayerRepository playerRepository)
    {
        _playerContext = playerContext;
        _roomRepository = roomRepository;
        _playerRepository = playerRepository;
    }

    public async Task InitializePlayerFromRoleAsync(Player player) // not added to Db yet
    {
        var room = await _roomRepository.GetRoomFromName(player.GameId, nameof(RoomsTemplate.MiddleCorridor));
        player.CurrentGameRoomId = room.Id;

        var item = new Item()
        {
            Id = Guid.NewGuid(),
            ItemType = ItemType.Wrench,
            OwnerId = player.Id,
        };

        await _playerContext.Items.AddAsync(item);

        await _playerContext.SaveChangesAsync();
    }

    public async Task TickPlayer(Guid playerId)
    {
        // Ticker pour chaque skill ? 
    }

    public async Task TickPlayerFromRoleAsync(Player player)
    {

    }
}
