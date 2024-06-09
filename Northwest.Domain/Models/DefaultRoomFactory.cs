using Northwest.Persistence.Entities;

namespace Northwest.Domain.Models;

public static class DefaultRoomFactory
{
    public static Tuple<List<Room>, List<AdjacentRoom>> CreateAndInitializeNewRoomsAndConnections(Guid gameId)
    {
        var defaultRooms = RoomsTemplate.ReadSerializedDefaultRooms();
        InitializeDefaultRoomIds(gameId, defaultRooms);
        var connections = CreateAndInitializeConnections(gameId, defaultRooms);

        var roomsAndAdjacents = new Tuple<List<Room>, List<AdjacentRoom>>(defaultRooms, connections);
        return roomsAndAdjacents;
    }

    private static void InitializeDefaultRoomIds(Guid gameId, List<Room> defaultRooms)
    {
        foreach (var room in defaultRooms)
        {
            room.Id = Guid.NewGuid();
            room.GameId = gameId;
        }
    }

    private static List<AdjacentRoom> CreateAndInitializeConnections(Guid gameId, List<Room> initializedRooms, bool isLandmass = false) // landmass rooms connections are initialized in RoomTemplate
    {
        if (initializedRooms.Any(x => x.Id == Guid.Empty || x.GameId == Guid.Empty)) throw new Exception("Rooms were not initializedCorrectly");

        var connections = new List<AdjacentRoom>();
        foreach (var room in initializedRooms)
        {
            foreach (string adjacentName in room.AdjacentRoomNames)
            {
                var adjRoomEntity = initializedRooms.FirstOrDefault(x => x.Name.Equals(adjacentName)) 
                    ?? throw new Exception($"adjacent room not found : {adjacentName}");

                var connection = new AdjacentRoom
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
