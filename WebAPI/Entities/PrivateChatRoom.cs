using WebAPI.Interfaces;

namespace WebAPI.Entities;

public class PrivateChatRoom : IEntity
{
    public Guid Id { get; set; }
    public string ChatRoomName { get; set; } = string.Empty;
}
