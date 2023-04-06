using System.Data;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using WebAPI.Db_Models;
using WebAPI.Dummies;
using WebAPI.Entities;
using WebAPI.Interfaces;
using WebAPI.Models;
using WebAPI.Repository;

namespace WebAPI.Services
{
    public class GameMakerService : IGameMakerService
    {
        private readonly IGameMakerRepository _gameMakerRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IStationRepository _stationRepository;
        private readonly PlayerContext _playerContext;
        private readonly ILandmassService _landmassService;

        public GameMakerService(PlayerContext playerContext,
            IGameMakerRepository gameMakerRepository,
            IRoomRepository roomRepository,
            IStationRepository stationRepository,
            ILandmassService landmassService)
        {
            _gameMakerRepository = gameMakerRepository;
            _roomRepository = roomRepository;
            _stationRepository = stationRepository;
            _playerContext = playerContext;
            _landmassService = landmassService;
        }

        public void CreateGame(NewGameInfo newGameInfo)
        {
            Guid gameGuid = Guid.NewGuid();

            Game newGame = new Game()
            {
                Active = true,
                Id = gameGuid,
                NextTick = DateTime.UtcNow.AddSeconds(CycleManagerService.TimeBetweenTicksInSeconds),
            };

            newGameInfo.Game = newGame;

            _playerContext.Games.Add(newGame);
            _roomRepository.CreateNewRooms(gameGuid);

            // Need to initialize rooms before players, because player construction requires a GameRoomId.
            InitializePlayers(newGameInfo);
            InitializeItems(newGameInfo);

            // Stations are tied to rooms, currently, only in services that interact with them in a hardcoded way. - So stations can be created independently of rooms
            _stationRepository.CreateAndAddStationsToDb(gameGuid);

            //
            //InitializeSerializedLandmass(newGameInfo);
            //InitializeLandmassLayoutAndRooms(newGameInfo);

            _playerContext.SaveChanges();
        }

        public void CreateDummyGame()
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
                           RoleType = Enums.RoleType.Commander,
                           Name = "BenPlayer"
                    },

                    new PlayerSelections() // fred
                    {
                           UserId = DummyValues.RealFred.Id,
                           RoleType = Enums.RoleType.Medic,
                           Name = "FredPlayer",
                    },
                },
            };
            CreateGame(info);
        }

        public void InitializeSerializedLandmass(NewGameInfo infos)
        {
            var newLandmass = new Landmass()
            {
                Id = Guid.NewGuid(),
                GameId = infos.Game.Id,
                SerializedLandmassLayout = string.Empty,
            };
            _playerContext.Landmass.Add(newLandmass);
            _playerContext.SaveChanges();
        }

        public void InitializePlayers(NewGameInfo info)
        {
            /* 
              Will need to clarify what gets initialized before what.
              for example, class cannot depend on starting room(for knowing which bonuses is applying), and starting room also depend on class. One needs to be
              created before the other. 
            */
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
                    Profession = Enums.RoleType.Commander,
                    X = 0f,
                    Y = 0f,
                    Z = 0f,
                };

                InitializePlayerRoleSettings(newPlayer);

                //Starting room might change according to some variables. 
                Guid roomId = GetStartingRoomId(newPlayer, info);
                newPlayer.CurrentGameRoomId = roomId;
                _playerContext.Players.Add(newPlayer);
            }
        }

        public void InitializePlayerRoleSettings(Player player)
        {
            // modify bonuses/skills  from classes 
            switch (player.Profession)
            {
                case Enums.RoleType.Commander: return;
                case Enums.RoleType.Medic: return;
                case Enums.RoleType.Engineer: return;
            }
            throw new Exception("Selected Profession has no settings defined.");
        }

        public Guid GetStartingRoomId(Player player, NewGameInfo info)
        {
            var rooms = _roomRepository.GetRoomsInGame(info.Game.Id);
            // some code that sets the startRoomId.
            return rooms.First().Id;
        }

        public void InitializeItems(NewGameInfo info)
        {
            List<Room> rooms = _roomRepository.GetRoomsInGame(info.Game.Id);

            var newItem = DummyValues.Item;
            newItem.OwnerId = rooms.First().Id;
            _playerContext.Items.Add(newItem);
            _playerContext.SaveChanges();
            // dummy value :

            // will need to know which room does what.
            //foreach (var room in rooms)
            //{
            //    switch (room.Name)
            //    {
            //        case "Kitchen1":
            //            {
            //                break;
            //            }
            //        default: { throw new Exception($"{room.Name} has not been found while initializing items."); };
            //    }
            //}
        }

        public void InitializeLandmassLayoutAndRooms(NewGameInfo info)
        {
            this.InitializeLandmassCards(info);
            _landmassService.NextLandmass(info.Game.Id);
        }

        private void InitializeLandmassCards(NewGameInfo infos)
        {
            // disons, pour l'instant,
            // 1 carte bonne,
            // 1 carte neutral,
            // 1 carte mauvaise
            Card goodCard = new Card()
            {
                Id = Guid.NewGuid(),
                IsDiscarded = false,
                GameId = infos.Game.Id,
                Name = "Expedition1",
                Value = Enums.CardValue.Positive
            };

            Card neutralCard = new Card()
            {
                Id = Guid.NewGuid(),
                IsDiscarded = false,
                GameId = infos.Game.Id,
                Name = "Kitchen1",
                Value = Enums.CardValue.Neutral
            };

            Card badCard = new Card()
            {
                Id = Guid.NewGuid(),
                IsDiscarded = false,
                GameId = infos.Game.Id,
                Name = "EntryHall",
                Value = Enums.CardValue.Negative
            };

            _playerContext.Cards.Add(goodCard);
            _playerContext.Cards.Add(neutralCard);
            _playerContext.Cards.Add(badCard);
            _playerContext.SaveChanges();
        }
    }
}