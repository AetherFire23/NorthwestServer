using LandmassTests;
using Shared_Resources.Entities;
using Shared_Resources.Models;
using WebAPI.TestFolder;

namespace WebAPI.Landmasses;

public static class LandmassEntitiesInitializer
{
    public static Tuple<List<Room>, List<AdjacentRoom>> CreateNewDefaultLandmassRoomsAndConnections(LandmassLayout layout, Guid gameId)
    {
        List<Room> defaultLandmassRooms = RoomsTemplate.ReadSerializedDefaultRooms().Where(x => x.IsLandmass).ToList();
        InitializeIdsIntoLandmassRooms(defaultLandmassRooms, gameId);
        AddAjacentNamesIntoLandmassRooms(layout, defaultLandmassRooms);
        SetRoomPositionIntoLandmassRooms(layout, defaultLandmassRooms);

        //Order is important or else Ids wont work.
        List<AdjacentRoom> adjacents = CreateAdjacentRooms(defaultLandmassRooms, gameId);

        Tuple<List<Room>, List<AdjacentRoom>> landmassRoomsAndConnections = new Tuple<List<Room>, List<AdjacentRoom>>(defaultLandmassRooms, adjacents);
        return landmassRoomsAndConnections;
    }

    private static void InitializeIdsIntoLandmassRooms(List<Room> landmassRooms, Guid gameId)
    {
        foreach (Room room in landmassRooms)
        {
            room.Id = Guid.NewGuid();
            room.GameId = gameId;
            room.IsActive = true;
        }
    }

    private static void AddAjacentNamesIntoLandmassRooms(LandmassLayout layout, List<Room> defaultLandmassRooms)
    {
        foreach (Room room in defaultLandmassRooms)
        {
            RoomCell layoutRoomCell = layout.AllCels.First(x => x.Name == room.Name);
            var neighborsNames = layoutRoomCell.DoorConnections.Values.Select(x => x.Name).ToList();
            room.AdjacentRoomNames.AddRange(neighborsNames);
        }
    }

    private static List<AdjacentRoom> CreateAdjacentRooms(List<Room> defaultLandmassRooms, Guid gameId)
    {
        var adjacents = DefaultRoomFactory.CreateAndInitializeConnections(gameId, defaultLandmassRooms, true);
        return adjacents;
    }

    private static void SetRoomPositionIntoLandmassRooms(LandmassLayout layout, List<Room> defaultLandmassRooms)
    {
        foreach (var room in defaultLandmassRooms)
        {
            var roomCell = layout.AllRooms.First(x => x.Name == room.Name);
            room.X = roomCell.Position.X;
            room.Y = roomCell.Position.Y;
        }
    }
}