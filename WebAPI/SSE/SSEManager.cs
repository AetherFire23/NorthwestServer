using Shared_Resources.Entities;
using Shared_Resources.Enums;
using Shared_Resources.Models.SSE;
using WebAPI.Interfaces;
using WebAPI.Utils;

namespace WebAPI.SSE
{
    public class SSEManager : ISSEManager
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly IRoomRepository _roomRepository;
        public SSEManager(IPlayerRepository playerRepository,
            IRoomRepository roomRepository)
        {
            _playerRepository = playerRepository;
            _roomRepository = roomRepository;
        }

        //public async Task SendItemChangedOwnerEvent(Guid gameId)
        //{
        //    List<Guid> loggedPlayers = _clientManager.GetLoggedPlayers(gameId);

        //    var allRoomItems = await _roomRepository.GetItemsInAllRooms(gameId);

        //    foreach (var playerId in loggedPlayers)
        //    {
        //        var playerItems = await _playerRepository.GetOwnedItems(playerId);
        //        PlayerAndRoomItems playerAndItems = new PlayerAndRoomItems()
        //        {
        //            PlayerItems = playerItems,
        //            RoomItems = allRoomItems,
        //        };
        //        var clientData = new SSEData<PlayerAndRoomItems>(SSEType.RefreshItems, playerAndItems);
        //        await _clientManager.PushDataToPlayer(playerId, clientData);
        //    }
        //}

        //public async Task SendNewLogEvent(Log log)
        //{
        //    var loggedPlayers = _clientManager.GetLoggedPlayers(log.GameId);
        //    var playersWhoCanAccessLog = await _playerRepository.FilterPlayersWhoHaveAccessToLog(loggedPlayers, log);

        //    var clientData = new SSEData<Log>(SSEType.AddLog, log);
        //    foreach (var player in playersWhoCanAccessLog)
        //    {
        //        await _clientManager.PushDataToPlayer(player, clientData);
        //    }
        //}
    }
}
