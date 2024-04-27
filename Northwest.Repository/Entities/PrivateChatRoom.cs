namespace Northwest.Persistence.Entities;

public class PrivateChatRoom : IEntity
{
    public Guid Id { get; set; }
    public string ChatRoomName { get; set; } = string.Empty;
}
