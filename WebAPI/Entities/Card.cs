using WebAPI.Enums;

namespace WebAPI.Entities
{
    public class Card
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public string Name { get; set; }
        public CardValue Value { get; set; }
        public bool IsDiscarded { get; set; }
    }
}
