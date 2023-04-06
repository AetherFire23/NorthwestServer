
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

        public void TransferItem(Guid ownerId, Guid targetId, Guid itemId)
        {
            Item item = _playerRepository.GetItem(itemId);
            item.OwnerId = targetId;
            _playerContext.SaveChanges();
        }

        public void UpdatePosition(Player UnityPlayerModel)
        {
            Player player = _playerRepository.GetPlayer(UnityPlayerModel.Id);
            player.X = UnityPlayerModel.X;
            player.Y = UnityPlayerModel.Y;
            player.Z = UnityPlayerModel.Z;
            _playerContext.SaveChanges();
        }

        public ClientCallResult ChangeRoom(Guid playerId, string targetRoomName)
        {
            var player = _playerRepository.GetPlayer(playerId);

            var currentRoom = _playerContext.Rooms.First(x => x.Id == player.CurrentGameRoomId);

            var targetRoom = _playerContext.Rooms.First(x => x.Name == targetRoomName); // devrait pas etre avec le gameid aussi?

            bool connectionExists = _playerContext.AdjacentRooms.FirstOrDefault(x => x.RoomId == currentRoom.Id && x.AdjacentId == targetRoom.Id) is not null;

            if (connectionExists)
            {
                player.CurrentGameRoomId = targetRoom.Id;
                _playerContext.SaveChanges();

                _gameActionsRepository.ChangeRoomAction(player, currentRoom, targetRoom);

                return ClientCallResult.Success;
            }

            return ClientCallResult.Failure;
        }
    }
}
