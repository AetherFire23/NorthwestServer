using WebAPI.Db_Models;
using WebAPI.DTOs;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly PlayerContext _playerContext;

        public RoomRepository(PlayerContext playerContext)
        {
            _playerContext = playerContext;
        }

        public RoomDTO GetRoomDTO(Guid roomId)
        {
            Room requestedRoom = _playerContext.Rooms.FirstOrDefault(room => room.Id == roomId);

            if (requestedRoom is null)
            {
                return new RoomDTO();
            }

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

        public void CreateNewRooms()
        {

            Guid gameId = Guid.NewGuid();

          //  Guid kitchen1ID = new Guid("c4ac05eb-8ad5-4320-8843-cb41a6906bc6");
            Guid kitchen2ID = new Guid("bf3accf0-a319-48c5-8c64-2a045d4b16e5");
            Guid entryHallID = new Guid("80998e1e-15ba-46bb-a1a8-b8b8c89c7004");
            var GetTemplate = BuildLevelTemplate();

            // Get default rooms 
            var defaultRooms = typeof(LevelTemplate).GetProperties()
                .Select(x => x.GetValue(GetTemplate))
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
                };
                newRooms.Add(newDbRoom);
            }

            // Create room connections : la premier join sert à faire correspondre la salle par defaut + la salle d'initialisation.
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

        public LevelTemplate BuildLevelTemplate()
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
                    },
                    RoomType = RoomType.Second
                },
            };
            return levelTemplate;
        }
    }
}
