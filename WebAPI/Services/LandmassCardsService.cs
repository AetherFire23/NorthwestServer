using Shared_Resources.Entities;
using Shared_Resources.Models;
using WebAPI.Interfaces;
using WebAPI.TestFolder;
using WebAPI.Utils;

namespace WebAPI.Services;

public class LandmassCardsService : ILandmassCardsService
{
    private readonly ILandmassCardsRepository _landmassCardsRepository;
    private readonly PlayerContext _playerContext;

    public LandmassCardsService(ILandmassCardsRepository landmassCardsRepository, PlayerContext playerContext)
    {
        _landmassCardsRepository = landmassCardsRepository;
        _playerContext = playerContext;
    }

    public async Task<List<string>> DrawNextLandmassRoomNames(Guid gameId) // rajouter un parameter pour pouvoir 
    {
        // Draw 7 cards, reshuffle if empty...
        List<Card> landmassCards = await _landmassCardsRepository.GetLandmassCardsAsync(gameId);
        List<Card> drawnCards = new List<Card>();

        // would need layouts of different room count to work 
        while (drawnCards.Count < 7) // warning  : is absolutely not generic since amount of cards needs to match amount of rooms.
        {
            if (landmassCards.All(x => x.IsDiscarded))
            {
                // When reshuffling, I dont know if landmassCards are updated...
                await ReshuffleLandmassCardsExceptDrawnCards(gameId, drawnCards);
                landmassCards = await _landmassCardsRepository.GetLandmassCardsAsync(gameId);
                continue;
            }


            //  drawnCards.Add(randomCard);
        }

        var drawnRoomNames = drawnCards.Select(x => x.Name).ToList();
        return drawnRoomNames;
    }

    public async Task<List<string>> DrawNextLandmassRoomNames2(Guid gameId, LandmassLayout layout)
    {
        List<Card> landmassCards = await _landmassCardsRepository.GetLandmassCardsAsync(gameId);
        List<Card> drawnCards = new List<Card>();

        while (drawnCards.Count < layout.AllRooms.Count)
        {
            await ReshuffleLandmassCardsIfNeededExceptDrawnCards(gameId, landmassCards, drawnCards);
            drawnCards.Add(await DrawRandomCard(landmassCards));
        }

        var drawnRoomNames = drawnCards.Select(x => x.Name).ToList();
        return drawnRoomNames;
    }

    // Initialize rooms from landmass cards.
    public async Task InitializeLandmassCards(Guid gameId)
    {
        var defaultRooms = RoomsTemplate.ReadSerializedDefaultRooms().Where(x => x.IsLandmass).ToList();

        var defaultLandmassCards = defaultRooms.Select(x => new Card()
        {
            Id = Guid.NewGuid(),
            IsDiscarded = false,
            GameId = gameId,
            Name = x.Name,
            CardImpact = x.CardImpact
        })
        .ToList();

        await _playerContext.AddRangeAsync(defaultLandmassCards);
        await _playerContext.SaveChangesAsync();
    }

    private async Task ReshuffleLandmassCardsExceptDrawnCards(Guid gameId, List<Card> cardsCurrentlyBeingDrawn)
    {
        var landmassCards = await _landmassCardsRepository.GetLandmassCardsAsync(gameId);
        if (landmassCards.Any(x => !x.IsDiscarded)) throw new Exception("Must shuffle cards only when there are no valid cards left");

        foreach (var card in landmassCards)
        {
            // do not switch on already drawn cards. 
            if (cardsCurrentlyBeingDrawn.Contains(card)) continue;

            card.IsDiscarded = false;
        }

        await _playerContext.SaveChangesAsync();
    }

    private async Task<Card> DrawRandomCard(List<Card> landmassCards)
    {
        var freeCards = landmassCards.Where(x => !x.IsDiscarded).ToList();
        var randomIndex = Random.Shared.Next(0, freeCards.Count);
        Card randomCard = freeCards[randomIndex];
        randomCard.IsDiscarded = true;
        return randomCard;
    }

    private async Task<List<Card>> ReshuffleLandmassCardsIfNeededExceptDrawnCards(Guid gameId, List<Card> landmassCards, List<Card> drawnCards)
    {
        if (landmassCards.All(x => x.IsDiscarded))
        {
            await ReshuffleLandmassCardsExceptDrawnCards(gameId, drawnCards);
            landmassCards = await _landmassCardsRepository.GetLandmassCardsAsync(gameId);
        }

        return landmassCards;
    }
}