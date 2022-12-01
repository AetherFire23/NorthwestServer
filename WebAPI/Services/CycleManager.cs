﻿using WebAPI.Db_Models;
using WebAPI.Entities;
using WebAPI.Game_Actions;
using WebAPI.Interfaces;

namespace WebAPI.Services
{
    public class CycleManager : ICycleManager
    {
        private int _offsetInSeconds = 500;
        private readonly PlayerContext _playerContext;
        private readonly IGameRepository _gameRepository;
        private readonly IPlayerRepository _playerRepository;
        public CycleManager(PlayerContext playerContext ,IGameRepository gameRepository, IPlayerRepository playerRepository)
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
                _playerContext.GameActions.Add(ga);
                _playerContext.TriggerNotifications.Add(notif);
            }

            game.NextTick = DateTime.UtcNow.AddSeconds(this._offsetInSeconds);

            _playerContext.SaveChanges();
            Console.WriteLine("Game Has ticked");
        }
    }
}
