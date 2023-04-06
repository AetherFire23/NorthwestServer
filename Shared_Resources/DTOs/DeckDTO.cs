using Shared_Resources.Entities;

namespace Shared_Resources.DTOs
{
    public class DeckDTO
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public List<Card> Cards { get; set; }
    }
}
