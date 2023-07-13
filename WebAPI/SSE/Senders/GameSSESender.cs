using Shared_Resources.Entities;
using Shared_Resources.Enums;
using Shared_Resources.Models.SSE;
using WebAPI.Interfaces;
using WebAPI.SSE.ClientManagers;

namespace WebAPI.SSE
{
    public class GameSSESender : SSESenderBase<GameSSEClientManager>, IGameSSESender
    {
        private readonly GameSSEClientManager _gameSSEClientManager;
        private readonly IPlayerRepository _playerRepository;
        private readonly IRoomRepository _roomRepository;

        public GameSSESender(IPlayerRepository playerRepository,
            IRoomRepository roomRepository,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _playerRepository = playerRepository;
            _roomRepository = roomRepository;
            _gameSSEClientManager = serviceProvider.GetSSEManager<GameSSEClientManager>();
        }

        public async Task SendItemChangedOwnerEvent(Guid gameId)
        {
            // refresh item and room items
            var loggedPlayers = _gameSSEClientManager.GetLoggedInPlayers(gameId);
            var allRoomItems = await _roomRepository.GetItemsInAllRooms(gameId);

            foreach (var player in loggedPlayers)
            {
                var playerItems = await _playerRepository.GetOwnedItems(player.Id);
                PlayerAndRoomItems playerAndItems = new PlayerAndRoomItems()
                {
                    PlayerItems = playerItems,
                    RoomItems = allRoomItems,
                };
                var clientData = new SSEData(SSEType.RefreshItems, playerAndItems);
                await _gameSSEClientManager.PushDataToSubscriber(player.Id, clientData);
            }
        }

        public async Task SendNewLogEvent(Log log)
        {
            var loggedPlayerIds = (_gameSSEClientManager.GetLoggedInPlayers(log.GameId))
                .Select(x => x.Id)
                .ToList();

            var playersWhoCanAccessLog = await _playerRepository.FilterPlayersWhoHaveAccessToLog(loggedPlayerIds, log);

            var clientData = new SSEData(SSEType.AddLog, log);
            foreach (var player in playersWhoCanAccessLog)
            {
                await _gameSSEClientManager.PushDataToSubscriber(player, clientData);
            }
        }
    }
}
