using Shared_Resources.Entities;
using Shared_Resources.Models.SSE;
using System;

namespace WebAPI.Utils
{
    public struct PlayerStruct : ISSESubscriber
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }

        public PlayerStruct(Guid playerId, Guid gameId)
        {
            Id = playerId;
            GameId = gameId;
        }

        public PlayerStruct(Player player)
        {
            this.Id = player.Id;
            this.GameId = player.GameId;
        }

        public PlayerStruct(Guid playerId)
        {
            this.Id = playerId;
            GameId = Guid.Empty;
        }
    }
}
