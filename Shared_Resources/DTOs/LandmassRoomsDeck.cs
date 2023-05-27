

using Shared_Resources.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Shared_Resources.DTOs
{
    public class LandmassRoomsDeck // obsolet  I think
    {
        public List<Card> AllCards;
        public LandmassRoomsDeck(List<Card> allCards)
        {
            AllCards = allCards;
        }
        public List<Card> Available => AllCards.Where(x => x.IsDiscarded == false).ToList();
        public List<Card> Discarded => AllCards.Where(x=> x.IsDiscarded).ToList();
        public List<Card> Positive => AllCards.Where(x => x.CardImpact == Enums.CardImpact.Positive).ToList();
        public List<Card> Neutral => AllCards.Where(x => x.CardImpact == Enums.CardImpact.Neutral).ToList();
        public List<Card> Negative => AllCards.Where(x => x.CardImpact == Enums.CardImpact.Negative).ToList();
    }
}
