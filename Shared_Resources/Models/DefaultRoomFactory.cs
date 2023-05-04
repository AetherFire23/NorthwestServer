using Newtonsoft.Json;
using Shared_Resources.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Shared_Resources.Models
{
    public static class DefaultRoomFactory
    {
        public static Tuple<List<Room>, List<AdjacentRoom>> CreateAndInitializeNewRoomsAndConnections(Guid gameId)
        {
            List<Room> defaultRooms = RoomTemplate2.ReadSerializedDefaultRooms();
            InitializeDefaultRoomIds(gameId, defaultRooms);
            List<AdjacentRoom> connections = CreateAndInitializeConnections(gameId, defaultRooms);

            var tuple = new Tuple<List<Room>, List<AdjacentRoom>>(defaultRooms, connections);
            return tuple;
        }

        public static void InitializeDefaultRoomIds(Guid gameId, List<Room> defaultRooms)
        {
            foreach (var room in defaultRooms)
            {
                room.Id = Guid.NewGuid();
                room.GameId = gameId;
            }
        }

        public static List<AdjacentRoom> CreateAndInitializeConnections(Guid gameId, List<Room> initializedRooms, bool isLandmass = false) // landmass rooms connections are initialized elsewhere.
        {
            if (initializedRooms.Any(x => x.Id == Guid.Empty || x.GameId == Guid.Empty)) throw new Exception("Rooms were not initializedCorrectly");

            List<AdjacentRoom> connections = new List<AdjacentRoom>();
            foreach (var room in initializedRooms)
            {
                foreach (var adjacentName in room.AdjacentRoomNames)
                {
                    var adjRoomEntity = initializedRooms.FirstOrDefault(x => x.Name.Equals(adjacentName));

                    if (adjRoomEntity is null) throw new Exception($"adjacent room not found : {adjacentName}");

                    AdjacentRoom connection = new AdjacentRoom()
                    {
                        Id = Guid.NewGuid(),
                        RoomId = room.Id,
                        AdjacentId = adjRoomEntity.Id,
                        GameId = gameId,
                        IsLandmassConnection = isLandmass
                    };

                    connections.Add(connection);
                }
            }
            return connections;
        }
    }
}
