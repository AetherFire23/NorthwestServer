namespace Shared_Resources.Entities
{
    public class PrivateChatRoomParticipant
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Guid ParticipantId { get; set; }
    }
}
