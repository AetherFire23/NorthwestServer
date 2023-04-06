using WebAPI.Entities;

namespace WebAPI.DTOs
{
    public class LandmassCards
    {
        public List<Card> AllCards;
        public LandmassCards(List<Card> allCards)
        {
            AllCards = allCards;
        }
        public List<Card> Available => AllCards.Where(x => x.IsDiscarded == false).ToList();
        public List<Card> Discarded => AllCards.Where(x=> x.IsDiscarded).ToList();
        public List<Card> Positive => AllCards.Where(x => x.Value == Enums.CardValue.Positive).ToList();
        public List<Card> Neutral => AllCards.Where(x => x.Value == Enums.CardValue.Neutral).ToList();
        public List<Card> Negative => AllCards.Where(x => x.Value == Enums.CardValue.Negative).ToList();
    }
}
