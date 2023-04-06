using WebAPI.Db_Models;
using WebAPI.DTOs;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly PlayerContext _playerContext;
        private readonly IRoomRepository _roomRepository;

        public RoomRepository(PlayerContext playerContext)
        {
            _playerContext = playerContext;
        }

        public List<Room> GetAllLandmassRooms(Guid gameId)
        {
            var rooms = _playerContext.Rooms.Where(r => r.IsLandmass && r.GameId == gameId).ToList();
            return rooms;
        }

        public List<Room> GetAllActiveLandmassRooms(Guid gameId)
        {
            var rooms = _playerContext.Rooms.Where(r => r.IsActive && r.IsActive && r.GameId == gameId).ToList();
            return rooms;
        }

        public Room GetRoomById(Guid roomId)
        {
            var room = _playerContext.Rooms.FirstOrDefault(r => r.Id == roomId);
            return room;
        }


        public void RemoveFromAllConnectedRooms(Guid roomId)
        {
            var connections = _playerContext.AdjacentRooms.Where(x => x.RoomId == roomId || x.AdjacentId == roomId).ToList();
            _playerContext.RemoveRange(connections);
            if (connections is null) return;

            _playerContext.SaveChanges();
        }

        public List<Room> GetRoomsInGame(Guid gameId)
        {
            var rooms = _playerContext.Rooms.Where(x => x.GameId == gameId).ToList();
            return rooms;
        }

        public Room GetRoomFromName(Guid gameId, string roomName)
        {
            var room = _playerContext.Rooms.FirstOrDefault(r => r.Name == roomName);
            return room;
        }

        public RoomDTO GetRoomDTO(Guid roomId)
        {
            Room requestedRoom = GetRoomById(roomId);


            var playersInRoom = _playerContext.Players.Where(player => player.CurrentGameRoomId == roomId).ToList();

            var items = _playerContext.Items.Where(item => item.OwnerId == roomId).ToList();

            RoomDTO roomDTO = new RoomDTO()
            {
                Id = requestedRoom.Id,
                RoomType = requestedRoom.RoomType,
                Items = items,
                Players = playersInRoom,
                Name = requestedRoom.Name,
                GameId = requestedRoom.GameId,
            };

            return roomDTO;
        }

        /// <summary>
        /// Initializes level with the given gameId
        /// </summary>
        /// <param name="gameId"></param>
        public void CreateNewRooms(Guid gameId)
        {
            //  Guid kitchen1ID = new Guid("c4ac05eb-8ad5-4320-8843-cb41a6906bc6");
            Guid kitchen2ID = new Guid("bf3accf0-a319-48c5-8c64-2a045d4b16e5");
            Guid entryHallID = new Guid("80998e1e-15ba-46bb-a1a8-b8b8c89c7004");
            var levelTemplate = BuildLevelTemplate();

            // Get default rooms 
            var defaultRooms = typeof(LevelTemplate).GetProperties()
                .Select(x => x.GetValue(levelTemplate))
                .Select(x => (RoomTemplate)x).ToList();


            // Initialize new Rooms 
            List<Room> newRooms = new List<Room>();
            for (int i = 0; i < defaultRooms.Count; i++)
            {
                var defaultRoom = defaultRooms[i];
                Room newDbRoom = new Room()
                {
                    Id = Guid.NewGuid(),
                    GameId = gameId,
                    Name = defaultRoom.Name,
                    RoomType = defaultRoom.RoomType,
                    IsLandmass = false,
                };
                newRooms.Add(newDbRoom);
            }

            // Create room connections : la premier join sert à faire correspondre la salle par defaut + la salle a initialiser.
            //                           Le deuxième join sert à faire correspondre le nom adjacent de la pièce à la pièce initée (impossible d'obtenir l'ID de la pièce par défaut).
            List<AdjacentRoom> adjacentRooms = newRooms.Join(defaultRooms,
                nr1 => nr1.Name,
                dr1 => dr1.Name,
                (nr1, dr1) => dr1.AdjacentNames.Join(newRooms,
                dr => dr,
                nr => nr.Name,
                (dr, nr) => new AdjacentRoom()
                {
                    Id = Guid.NewGuid(),
                    RoomId = nr1.Id,
                    AdjacentId = nr.Id
                })).SelectMany(x => x).ToList();


            _playerContext.Rooms.AddRange(newRooms);
            _playerContext.AdjacentRooms.AddRange(adjacentRooms);

            _playerContext.SaveChanges(); // saved ! 
        }



        private LevelTemplate BuildLevelTemplate()
        {
            LevelTemplate levelTemplate = new()
            {
                EntryHall = new RoomTemplate()
                {
                    Id = Guid.Empty,
                    Name = nameof(LevelTemplate.EntryHall),
                    AdjacentNames = new List<string>()
                    {
                        nameof(LevelTemplate.Kitchen1),
                    },
                    RoomType = RoomType.Start
                },

                Kitchen1 = new RoomTemplate()
                {
                    Id = Guid.Empty,
                    Name = nameof(LevelTemplate.Kitchen1),
                    AdjacentNames = new List<string>()
                    {
                        nameof(LevelTemplate.EntryHall),
                        nameof(LevelTemplate.Expedition1),

                    },
                    RoomType = RoomType.Second
                },

                Expedition1 = new RoomTemplate()
                {
                    Id = Guid.Empty,
                    Name = nameof(LevelTemplate.Expedition1),
                    AdjacentNames = new List<string>()
                    {
                        nameof(LevelTemplate.Kitchen1),
                    },
                    RoomType = RoomType.Start
                },



            };
            return levelTemplate;
        }
    }
}
