using Shared_Resources.Entities;
using Shared_Resources.Enums;
using Shared_Resources.Models.SSE;
using WebAPI.Interfaces;
using WebAPI.Utils;

namespace WebAPI.Services
{
    public class SSEManager : ISSEManager
    {
        private readonly ISSEClientManager _clientManager;
        private readonly IPlayerRepository _playerRepository;
        private readonly IRoomRepository _roomRepository;
        public SSEManager(ISSEClientManager clientManager,
            IPlayerRepository playerRepository,
            IRoomRepository roomRepository)
        {
            _clientManager = clientManager;
            _playerRepository = playerRepository;
            _roomRepository = roomRepository;
        }

        public async Task SendItemChangedOwnerEvent(Guid gameId)
        {
            List<Guid> loggedPlayers = await _clientManager.GetLoggedPlayers(gameId);

            var allRoomItems = await _roomRepository.GetItemsInAllRooms(gameId);

            foreach (var playerId in loggedPlayers)
            {
                var playerItems = await _playerRepository.GetOwnedItems(playerId);
                PlayerAndRoomItems playerAndItems = new PlayerAndRoomItems()
                {
                    PlayerItems = playerItems,
                    RoomItems = allRoomItems,
                };
                var clientData = new SSEData<PlayerAndRoomItems>(SSEEventType.RefreshItems, playerAndItems);
                await _clientManager.PushDataToPlayer(playerId, clientData);
            }
        }
    }
}
