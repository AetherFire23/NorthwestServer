using Shared_Resources.Entities;
using Shared_Resources.Interfaces;
using Shared_Resources.Models.SSE;

namespace WebAPI.SSE.Subscribers
{
    public class PlayerSubscriber : ISSESubscriber
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }

        public PlayerSubscriber(Guid playerId, Guid gameId)
        {
            Id = playerId;
            GameId = gameId;
        }

        public PlayerSubscriber(Player player)
        {
            Id = player.Id;
            GameId = player.GameId;
        }

        public PlayerSubscriber(Guid playerId)
        {
            Id = playerId;
            GameId = Guid.Empty;
        }
    }
}
