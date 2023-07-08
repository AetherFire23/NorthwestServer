
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shared_Resources.Entities;
using Shared_Resources.Models;
using Shared_Resources.Models.SSE;
using WebAPI.Game_Actions;
using WebAPI.Interfaces;

namespace WebAPI.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly PlayerContext _playerContext;
        private readonly IPlayerRepository _playerRepository;
        private readonly IGameActionsRepository _gameActionsRepository;
        private readonly ISSEClientManager _sseClientManager;
        private readonly ISSEManager _sseManager;
        public PlayerService(
            PlayerContext playerContext,
            IPlayerRepository playerRepository,
            IGameActionsRepository gameActionsRepository,
            ISSEClientManager sseClientManager,
            ISSEManager sseManager)
        {
            _playerContext = playerContext;
            _playerRepository = playerRepository;
            _gameActionsRepository = gameActionsRepository;
            _sseClientManager = sseClientManager;
            _sseManager = sseManager;
        }

        public async Task TransferItem(Guid targetId, Guid itemId, Guid gameId)
        {
            Item item = await _playerRepository.GetItemAsync(itemId);
            item.OwnerId = targetId;

            await _playerContext.SaveChangesAsync();
            await _sseManager.SendItemChangedOwnerEvent(gameId);
        }

        public async Task UpdatePositionAsync(Guid playerId, float x, float y)
        {
            Player player = await _playerRepository.GetPlayerAsync(playerId);
            player.X = x;
            player.Y = y;
            await _playerContext.SaveChangesAsync();
        }

        public async Task<ClientCallResult> ChangeRoomAsync(Guid playerId, string targetRoomName)
        {
            var player = await _playerRepository.GetPlayerAsync(playerId);
            var currentRoom = await _playerContext.Rooms.FirstAsync(x => x.Id == player.CurrentGameRoomId);
            var targetRoom = await _playerContext.Rooms.FirstAsync(x => x.Name == targetRoomName); // devrait pas etre avec le gameid aussi?
            bool connectionExists = await _playerContext.AdjacentRooms.FirstOrDefaultAsync(x => x.RoomId == currentRoom.Id && x.AdjacentId == targetRoom.Id) is not null;

            if (connectionExists) // should benefit from inversion 
            {
                player.CurrentGameRoomId = targetRoom.Id;
                await _playerContext.SaveChangesAsync();

                var log = new Log()
                {
                    Id = Guid.NewGuid(),
                    Created = DateTime.UtcNow,
                    CreatedBy = player.Name,
                    EventText = $"Player {player.Name}, who was in room {currentRoom.Name}, went into {targetRoom.Name}",
                    IsPublic = true,
                    RoomId = targetRoom.Id,
                    TriggeringPlayerId = player.Id,
                    GameId = player.GameId,
                };

                var gameAction = new GameAction()
                {
                    Id = Guid.NewGuid(),
                    Created = DateTime.UtcNow,
                    CreatedBy = player.Name,
                    GameId = player.GameId,
                    GameActionType = Shared_Resources.Enums.GameActionType.RoomChanged,
                    SerializedProperties = JsonConvert.SerializeObject(log),
                };

                await _playerContext.AddAsync(gameAction);
                await _playerContext.AddAsync(log);
                await _playerContext.SaveChangesAsync();

                //_gameActionsRepository.ChangeRoomAction(player, currentRoom, targetRoom);

                return ClientCallResult.Success;
            }

            // else fail
            string message = $"Connection between {currentRoom.Name} and {targetRoom.Name} did not exist. If room is intended to be destroyed, use .IsBlockedConnection";


            return new ClientCallResult()
            {
                Message = message,
                IsSuccessful = false,
            };
        }
    }
}
