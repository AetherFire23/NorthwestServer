using WebAPI.Entities;

namespace WebAPI.DTOs
{
    public class DeckDTO
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public List<Card> Cards { get; set; }
    }
}
