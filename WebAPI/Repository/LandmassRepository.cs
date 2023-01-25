using Newtonsoft.Json.Linq;
using WebAPI.Db_Models;
using WebAPI.DTOs;
using WebAPI.Entities;
using WebAPI.Interfaces;

namespace WebAPI.Repository
{
    public class LandmassRepository : ILandmassRepository
    {
        private readonly PlayerContext _playerContext;
        public LandmassRepository(PlayerContext playerContext)
        {
            _playerContext = playerContext;
        }

        // ForService
        public void NextLandmass(Guid gameId)
        {
            var currentSetup = GetCurrentLandmassDeckSetup(gameId);

            List<Card> drawnRoomcards = DrawNextLandmassCards(currentSetup);

            // les rooms sont jamais added ou removed, sont juste modifies selon le landmass actif.

            var landmassRooms = new List<Room>();
            
            foreach (var roomCard in drawnRoomcards)
            {
                var room = _playerContext.Rooms.FirstOrDefault(x => x.Name == roomCard.Name);
                landmassRooms.Add(room);
            }

            

            // je devrais evidemment faire du pairing de stations rendu la.
            // Je pense je vais faire un Dictionaire de <string, string> (roomName, stations)
            // ConcurrentDictionary is the way to go.
            // stationIds

            

            LandmassInfo landmassInfo = new LandmassInfo()
            {
                
            };

            SavePreviousLandmass(gameId);

            SaveCards(currentSetup);
        }

        public void SavePreviousLandmass(Guid gameId)
        {
            LandmassInfo info = new LandmassInfo()
            {

            };
        }

        public List<Card> DrawNextLandmassCards(DecksSetup setup)
        {
            //Some logic that determines which are drawn
            return null;
        }



        public DecksSetup GetCurrentLandmassDeckSetup(Guid gameId)
        {
            var allCards = _playerContext.Cards.Where(x => x.GameId == gameId).ToList();
            DecksSetup setup = new DecksSetup(allCards);
            return setup;
        }

        public void SaveCards(DecksSetup decksSetup)
        {
            foreach (var card in decksSetup._allCards)
            {
                var oldCard = _playerContext.Cards.FirstOrDefault(x => x.Id == card.Id);
                oldCard.Value = card.Value;
                oldCard.IsDiscarded = card.IsDiscarded;
            }

            _playerContext.SaveChanges();
        }
    }
}
