﻿using Shared_Resources.Entities;
using Shared_Resources.Enums;
using Shared_Resources.Models;
using System.Data;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using WebAPI.Dummies;
using WebAPI.Interfaces;
using WebAPI.Repository;
using WebAPI.Scratches;
using WebAPI.Strategies;

namespace WebAPI.Services
{
    public class GameMakerService : IGameMakerService
    {
        private readonly IGameMakerRepository _gameMakerRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IStationRepository _stationRepository;
        private readonly PlayerContext _playerContext;
        private readonly ILandmassService2 _landmassService;
        private readonly ILandmassCardsService _landmassCardsService;
        private readonly IServiceProvider _serviceProvider;
        public GameMakerService(PlayerContext playerContext,
            IGameMakerRepository gameMakerRepository,
            IRoomRepository roomRepository,
            IStationRepository stationRepository,
            ILandmassService2 landmassService, ILandmassCardsService landmassCardsService, IServiceProvider serviceProvider)
        {
            _gameMakerRepository = gameMakerRepository;
            _roomRepository = roomRepository;
            _stationRepository = stationRepository;
            _playerContext = playerContext;
            _landmassService = landmassService;
            _landmassCardsService = landmassCardsService;
            _serviceProvider = serviceProvider;
        }

        public async Task CreateDummyGame()
        {
            NewGameInfo info = new NewGameInfo()
            {
                Users = new List<User>()
                {
                    DummyValues.RealBen,
                    DummyValues.RealFred,
                },

                RoleChoices = new List<PlayerSelections>()
                {
                    new PlayerSelections() // ben
                    {
                           UserId = DummyValues.RealBen.Id,
                           RoleType = RoleType.Commander,
                           Name = "BenPlayer"
                    },

                    new PlayerSelections() // fred
                    {
                           UserId = DummyValues.RealFred.Id,
                           RoleType = RoleType.Medic,
                           Name = "FredPlayer",
                    },
                },
            };
            await CreateGame(info);
        }

        public async Task CreateGame(NewGameInfo newGameInfo)
        {
            Guid gameId = DummyValues.Game.Id;

            Game newGame = new Game()
            {
                Active = true,
                Id = gameId,
                NextTick = DateTime.UtcNow.AddSeconds(CycleManagerService.TimeBetweenTicksInSeconds),
            };

            newGameInfo.Game = newGame;

            _playerContext.Games.Add(newGame);
            await _playerContext.SaveChangesAsync();

            // Need to initialize rooms before players, because player construction requires a GameRoomId.

            await _roomRepository.CreateNewRooms(gameId);
            await InitializePlayers(newGameInfo);
            await InitializeItemsAsync(newGameInfo);
            await _landmassCardsService.InitializeLandmassCards(newGameInfo.Game.Id);

            // Stations are tied to rooms, currently, only in services that interact with them in a hardcoded way. - So stations can be created independently of rooms
            await _stationRepository.CreateAndAddStationsToDb(gameId);
            await _landmassService.AdvanceToNextLandmass(gameId);
            await _playerContext.SaveChangesAsync();
        }

        public void InsertVeryDummyValues()
        {
            _playerContext.TriggerNotifications.Add(DummyValues.TriggerNotification1);
            _playerContext.Messages.Add(DummyValues.Message1);
            _playerContext.Messages.Add(DummyValues.Message2);
            _playerContext.PrivateChatRooms.Add(DummyValues.PrivateChatRoom);
            _playerContext.PrivateChatRoomParticipants.Add(DummyValues.PrivateChatRoomParticipant);
            _playerContext.Items.Add(DummyValues.Freditem);

            _playerContext.SaveChanges();
        }

        public async Task InitializePlayers(NewGameInfo info)
        {
            /* Will need to clarify what gets initialized before what.
              for example, class cannot depend on starting room(for knowing which bonuses is applying), and starting room also depend on class. One needs to be
              created before the other. */
            foreach (var user in info.Users)
            {
                PlayerSelections selection = info.RoleChoices.First(x => x.UserId == user.Id);
                var newPlayer = new Player()
                {
                    Id = user.Id, // users not saved yet in db, so can use user key as primary key. Will have to Guid.NewGuid() some day
                    ActionPoints = 0,
                    CurrentChatRoomId = user.Id,
                    CurrentGameRoomId = user.Id,
                    GameId = info.Game.Id,
                    HealthPoints = 0,
                    Name = selection.Name,
                    Profession = info.RoleChoices.First(x => x.UserId == user.Id).RoleType,
                    X = 0f,
                    Y = 0f,
                    Z = 0f,
                };

                await InitializePlayerRoleSettings(newPlayer);

                //Starting room might change according to some variables. 
                Guid roomId = await GetStartingRoomId(newPlayer, info);
                newPlayer.CurrentGameRoomId = roomId;
                _playerContext.Players.Add(newPlayer);
            }
        }

        public async Task InitializePlayerRoleSettings(Player player)
        {
            var roleStrategy = GetRoleStrategy(player.Profession);
            await roleStrategy.InitializePlayerFromRoleAsync(player);
        }

        public async Task<Guid> GetStartingRoomId(Player player, NewGameInfo info) // based on Role ?
        {
            var rooms = await _roomRepository.GetRoomsInGame(info.Game.Id);
            // some code that sets the startRoomId.
            var firstNotLandmass = rooms.First(x => !x.IsLandmass).Id;
            return firstNotLandmass;
        }

        public async Task InitializeItemsAsync(NewGameInfo info) // problems with disappearing items : check landmass creation that swaps the landmasses...
        {
            List<Room> rooms = await _roomRepository.GetRoomsInGame(info.Game.Id);

            var newItem = DummyValues.Item;
            newItem.OwnerId = rooms.First(x => !x.IsLandmass).Id;
            Item roomItem = new Item()
            {
                Id = Guid.NewGuid(),
                ItemType = ItemType.Hose,
                OwnerId = rooms.First(x => !x.IsLandmass).Id,
            };
            _playerContext.Items.Add(roomItem);
            _playerContext.Items.Add(newItem);
            _playerContext.Items.Add(DummyValues.GetRandomItem(newItem.OwnerId));
            _playerContext.Items.Add(DummyValues.GetRandomItem(newItem.OwnerId));
            await _playerContext.SaveChangesAsync();
        }

        public IRoleInitializationStrategy GetRoleStrategy(RoleType role)
        {
            var strategType = StrategyMapper.GetStrategyTypeByRole(role);
            var strategy = _serviceProvider.GetService(strategType) as IRoleInitializationStrategy;
            return strategy;
        }
    }
}
