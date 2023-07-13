﻿using Shared_Resources.Entities;
using Shared_Resources.Enums;
using Shared_Resources.Models;
using WebAPI.Interfaces;

namespace WebAPI.Strategies
{
    [RoleStrategy(RoleType.Cook)]
    public class CookRoleStrategy : IRoleInitializationStrategy
    {
        private readonly PlayerContext _playerContext;
        private readonly IRoomRepository _roomRepository;
        private readonly IPlayerRepository _playerRepository;
        public CookRoleStrategy(PlayerContext playerContext,
            IRoomRepository roomRepository,
            IPlayerRepository playerRepository)
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
}