using Newtonsoft.Json;
using Shared_Resources.DTOs;
using Shared_Resources.Entities;
using WebAPI.Landmasses;

namespace WebAPI.Repositories;

public class LandmassRepository
{
    private readonly PlayerContext _playerContext;
    public LandmassRepository(PlayerContext playerContext)
    {
        _playerContext = playerContext;
    }

    public void SaveLandmassLayout(Guid gameId, LandmassLayout layout)
    {
        JsonSerializerSettings jsonSettings = new JsonSerializerSettings();
        jsonSettings.PreserveReferencesHandling = PreserveReferencesHandling.All;
        string s = JsonConvert.SerializeObject(layout, jsonSettings);

        Landmass land = _playerContext.Landmass.First(l => l.GameId == gameId);
        land.SerializedLandmassLayout = s;
        _ = _playerContext.SaveChanges();
    }

    public LandmassLayout GetRandomLandmassLayout()
    {
        string serialized = File.ReadAllText("Layout.txt");
        List<LandmassLayout>? layouts = JsonConvert.DeserializeObject<List<LandmassLayout>>(serialized);
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
        List<Card> allCards = _playerContext.Cards.Where(x => x.GameId == gameId).ToList();
        LandmassRoomsDeck setup = new LandmassRoomsDeck(allCards);
        return setup;
    }

    public void SaveDecksSetup(LandmassRoomsDeck decksSetup)
    {
        foreach (Card card in decksSetup.AllCards)
        {
            Card? oldCard = _playerContext.Cards.FirstOrDefault(x => x.Id == card.Id);
            oldCard.CardImpact = card.CardImpact;
            oldCard.IsDiscarded = card.IsDiscarded;
        }
        _ = _playerContext.SaveChanges();
    }
}
