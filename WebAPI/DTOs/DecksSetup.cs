using WebAPI.Entities;

namespace WebAPI.DTOs
{
    public class DecksSetup
    {
        public List<Card> _allCards;
        public DecksSetup(List<Card> allCards)
        {
            _allCards = allCards;
        }
        public List<Card> Available => _allCards.Where(x => x.IsDiscarded == false).ToList();
        public List<Card> Discarded => _allCards.Where(x=> x.IsDiscarded).ToList();

        public List<Card> Positive => _allCards.Where(x => x.Value == Enums.CardValue.Positive).ToList();

        public List<Card> Neutral => _allCards.Where(x => x.Value == Enums.CardValue.Neutral).ToList();

        public List<Card> Negative => _allCards.Where(x => x.Value == Enums.CardValue.Negative).ToList();
    }
}
