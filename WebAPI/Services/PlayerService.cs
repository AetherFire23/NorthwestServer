
using Microsoft.EntityFrameworkCore;
using Shared_Resources.Entities;
using Shared_Resources.Models;
using WebAPI.Game_Actions;
using WebAPI.Interfaces;

namespace WebAPI.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly PlayerContext _playerContext;
        private readonly IPlayerRepository _playerRepository;
        private readonly IGameActionsRepository _gameActionsRepository;
        public PlayerService(PlayerContext playerContext, IPlayerRepository playerRepository, IGameActionsRepository gameActionsRepository)
        {
            _playerContext = playerContext;
            _playerRepository = playerRepository;
            _gameActionsRepository = gameActionsRepository;
        }

        public async Task TransferItem(Guid targetId, Guid itemId)
        {
            Item item = await _playerRepository.GetItemAsync(itemId);
            item.OwnerId = targetId;
            await _playerContext.SaveChangesAsync();
        }

        public async Task UpdatePositionAsync(Player UnityPlayerModel)
        {
            Player player = await _playerRepository.GetPlayerAsync(UnityPlayerModel.Id);
            player.X = UnityPlayerModel.X;
            player.Y = UnityPlayerModel.Y;
            player.Z = UnityPlayerModel.Z;
            _playerContext.SaveChanges();
        }

        public async Task<ClientCallResult> ChangeRoomAsync(Guid playerId, string targetRoomName)
        {
            var player = await _playerRepository.GetPlayerAsync(playerId);

            var currentRoom = await _playerContext.Rooms.FirstAsync(x => x.Id == player.CurrentGameRoomId);

            var targetRoom = await _playerContext.Rooms.FirstAsync(x => x.Name == targetRoomName); // devrait pas etre avec le gameid aussi?

            bool connectionExists = await _playerContext.AdjacentRooms.FirstOrDefaultAsync(x => x.RoomId == currentRoom.Id && x.AdjacentId == targetRoom.Id) is not null;

            if (connectionExists)
            {
                player.CurrentGameRoomId = targetRoom.Id;
                _playerContext.SaveChanges();

                _gameActionsRepository.ChangeRoomAction(player, currentRoom, targetRoom);

                return ClientCallResult.Success;
            }

            // else fail
            string message = $"Connection between {currentRoom.Name} and {targetRoom.Name} did not exist";


            return new ClientCallResult()
            {
                Message = message,
                IsSuccessful = false,
            };
        }
    }
}
