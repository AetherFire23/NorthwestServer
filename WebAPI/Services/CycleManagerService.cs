using WebAPI.Db_Models;
using WebAPI.Entities;
using WebAPI.Game_Actions;
using WebAPI.Interfaces;

namespace WebAPI.Services
{
    public class CycleManagerService : ICycleManagerService
    {
        public static int TimeBetweenTicksInSeconds = 5;
        private readonly PlayerContext _playerContext;
        private readonly IGameRepository _gameRepository;
        private readonly IPlayerRepository _playerRepository;
        public CycleManagerService(PlayerContext playerContext, IGameRepository gameRepository, IPlayerRepository playerRepository)
        {
            _playerContext = playerContext;
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
        }

        public void TickGame(Guid gameId)
        {
            var game = _gameRepository.GetGame(gameId);

            var playersInGame = _playerRepository.GetPlayersInGame(gameId);
            foreach (var player in playersInGame)
            {
                player.ActionPoints += 1;
                player.HealthPoints += 1;
                var ga = new GameAction()
                {
                    Id = Guid.NewGuid(),
                    Created = DateTime.UtcNow,
                    CreatedBy = "game",
                    GameId = gameId,
                    GameActionType = Enums.GameActionType.CycleTick,
                    SerializedProperties = String.Empty,
                    
                };
                var notif = new TriggerNotification()
                {
                    Id = Guid.NewGuid(),
                    Created = DateTime.UtcNow,
                    IsReceived = false,
                    GameActionId = ga.Id,
                    NotificationType = Enums.NotificationType.CycleChanged,
                    PlayerId = player.Id,
                    SerializedProperties = String.Empty,
                };

                Log log = new Log()
                {
                    Id = Guid.NewGuid(),
                    Created = DateTime.UtcNow,
                    CreatedBy = "Game",
                    EventText = "A cycle has magnificently ticked.",
                    TriggeringPlayerId = gameId,
                    IsPublic = true,
                    RoomId = Guid.Empty,
                    
                };

                _playerContext.Logs.Add(log);
                _playerContext.GameActions.Add(ga);
                _playerContext.TriggerNotifications.Add(notif);
            }

            game.NextTick = DateTime.UtcNow.AddSeconds(TimeBetweenTicksInSeconds);

            _playerContext.SaveChanges();
            Console.WriteLine("Game Has ticked");
        }
    }
}
