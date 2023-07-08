using Shared_Resources.Entities;

namespace WebAPI.Utils
{
    public struct PlayerStruct
    {
        public Guid PlayerId { get; }
        public Guid GameId { get; }

        public PlayerStruct(Guid playerId, Guid gameId)
        {
            PlayerId = playerId;
            GameId = gameId;
        }

        public PlayerStruct(Player player)
        {
            this.PlayerId = player.Id;
            this.GameId = player.GameId;
        }

        public PlayerStruct(Guid playerId)
        {
            this.PlayerId = playerId;
        }
    }
}
