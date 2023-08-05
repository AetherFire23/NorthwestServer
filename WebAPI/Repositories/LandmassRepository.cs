using Newtonsoft.Json;
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using WebAPI.Interfaces;
using WebAPI.TestFolder;

namespace WebAPI.Repository;

public class LandmassRepository : ILandmassRepository
{
    private readonly PlayerContext _playerContext;
    private readonly IRoomRepository _roomRepository;
    private readonly IStationRepository _stationRepository;
    public LandmassRepository(PlayerContext playerContext, IRoomRepository roomRepository,
        IStationRepository stationRepository)
    {
        _playerContext = playerContext;
        _roomRepository = roomRepository;
        _stationRepository = stationRepository;
    }

    public void SaveLandmassLayout(Guid gameId, LandmassLayout layout)
    {
        var jsonSettings = new JsonSerializerSettings();
        jsonSettings.PreserveReferencesHandling = PreserveReferencesHandling.All;
        string s = JsonConvert.SerializeObject(layout, jsonSettings);

        var land = _playerContext.Landmass.First(l => l.GameId == gameId);
        land.SerializedLandmassLayout = s;
        _ = _playerContext.SaveChanges();
    }

    public LandmassLayout GetRandomLandmassLayout()
    {
        string serialized = File.ReadAllText("Layout.txt");
        var layouts = JsonConvert.DeserializeObject<List<LandmassLayout>>(serialized);
        Random r = new Random();
        int randomIndex = r.Next(0, layouts.Count);
        LandmassLayout randomLayout = layouts[randomIndex];
        return randomLayout;
    }

    public void SavePreviousLandmass(Guid gameId)
    {
        _ = new LandmassInfo()
        {

        };
    }

    public LandmassRoomsDeck GetCurrentLandmassDeckSetup(Guid gameId)
    {
        var allCards = _playerContext.Cards.Where(x => x.GameId == gameId).ToList();
        LandmassRoomsDeck setup = new LandmassRoomsDeck(allCards);
        return setup;
    }

    public void SaveDecksSetup(LandmassRoomsDeck decksSetup)
    {
        foreach (var card in decksSetup.AllCards)
        {
            var oldCard = _playerContext.Cards.FirstOrDefault(x => x.Id == card.Id);
            oldCard.CardImpact = card.CardImpact;
            oldCard.IsDiscarded = card.IsDiscarded;
        }
        _ = _playerContext.SaveChanges();
    }
}
