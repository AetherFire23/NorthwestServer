using Shared_Resources.Entities;
using Shared_Resources.Enums;
using Shared_Resources.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebAPI.Scratches;
using WebAPI.Utils;

namespace WebAPI
{
    public class LandmassCardsService : ILandmassCardsService
    {
        private readonly ILandmassCardsRepository _landmassCardsRepository;
        private readonly PlayerContext _playerContext;

        public LandmassCardsService(ILandmassCardsRepository landmassCardsRepository, PlayerContext playerContext)
        {
            _landmassCardsRepository = landmassCardsRepository;
            _playerContext = playerContext;
        }

        public async Task<List<string>> DrawNextLandmassRoomNames(Guid gameId)
        {
            // Draw 7 cards, reshuffle if empty...
            List<Card> landmassCards = await _landmassCardsRepository.GetLandmassCards(gameId);
            List<Card> drawnCards = new List<Card>();

            while (drawnCards.Count < 7)
            {
                if (landmassCards.All(x => x.IsDiscarded))
                {
                    // When reshuffling, I dont know if landmassCards are updated...
                    await ReshuffleLandmassCards(gameId, drawnCards);
                    landmassCards = await _landmassCardsRepository.GetLandmassCards(gameId);
                    continue;
                }

                var freeCards = landmassCards.Where(x => !x.IsDiscarded).ToList();
                var randomIndex = RandomHelper.random.Next(0, freeCards.Count);
                Card randomCard = freeCards[randomIndex];
                randomCard.IsDiscarded = true;
                drawnCards.Add(randomCard);
            }

            var drawnRoomNames = drawnCards.Select(x => x.Name).ToList();
            return drawnRoomNames;
        }

        // Initialize rooms from landmass cards.
        public async Task InitializeLandmassCards(Guid gameId)
        {
            var defaultRooms = RoomTemplate2.ReadSerializedDefaultRooms().Where(x => x.IsLandmass).ToList();

            var defaultLandmassCards = defaultRooms.Select(x => new Card()
            {
                Id = Guid.NewGuid(),
                IsDiscarded = false,
                GameId = gameId,
                Name = x.Name,
                CardImpact = x.CardImpact
            })
            .ToList();

            _playerContext.AddRange(defaultLandmassCards);
            _playerContext.SaveChanges();
        }

        private async Task ReshuffleLandmassCards(Guid gameId, List<Card> cardsCurrentlyBeingDrawn)
        {
            var landmassCards = await _landmassCardsRepository.GetLandmassCards(gameId);
            if (landmassCards.Where(x => !x.IsDiscarded).Count() > 0) throw new Exception("Must shuffle cards only when there are no valid cards left");

            foreach (var card in landmassCards)
            {
                // do not switch on already drawn cards. 
                if (cardsCurrentlyBeingDrawn.Contains(card)) continue;

                card.IsDiscarded = false;
            }

            _playerContext.SaveChanges();
        }
    }
}