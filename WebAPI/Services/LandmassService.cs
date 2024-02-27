using WebAPI.Entities;
using WebAPI.Landmasses;
using WebAPI.Repositories;

namespace WebAPI.Services;

public class LandmassService
{
    private readonly PlayerContext _playerContext;
    private readonly RoomRepository _roomRepository;
    private readonly LandmassCardsService _landmassCardsService;

    public LandmassService(PlayerContext playerContext,
        RoomRepository roomRepository,
        LandmassCardsService landmassCardsService)
    {
        _roomRepository = roomRepository;
        _playerContext = playerContext;
        _landmassCardsService = landmassCardsService;
    }

    public async Task AdvanceToNextLandmass(Guid gameId) // entry point for switching landmasses
    {
        LandmassLayout layout2 = LandmassGetter.CreateNewLandmass();
        List<string> roomNames2 = await _landmassCardsService.DrawNextLandmassRoomNames2(gameId, layout2);
        LandmassGetter.InsertLandmassNamesInLayout(layout2, roomNames2);

        Tuple<List<Room>, List<AdjacentRoom>> s = LandmassEntitiesInitializer.CreateNewDefaultLandmassRoomsAndConnections(layout2, gameId);
        // ligne ici pour initializer les stations des landmasses I guess

        await WipeLandmassRoomsAndConnections(gameId);
        await _playerContext.Rooms.AddRangeAsync(s.Item1);
        await _playerContext.AdjacentRooms.AddRangeAsync(s.Item2);
        await _playerContext.SaveChangesAsync();
    }

    private async Task WipeLandmassRoomsAndConnections(Guid gameId) // faudra y penser
    {
        List<Room> landmassRooms = await _roomRepository.GetAllLandmassRoomsInGame(gameId);
        List<AdjacentRoom> connections = await _roomRepository.GetLandmassAdjacentRoomsAsync(gameId);

        _playerContext.Rooms.RemoveRange(landmassRooms);
        _playerContext.AdjacentRooms.RemoveRange(connections);
    }
}