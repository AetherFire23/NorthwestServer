using Shared_Resources.Interfaces;
using System;

namespace Shared_Resources.Entities;

public class PrivateChatRoom : IEntity
{
    public Guid Id { get; set; }
    public string ChatRoomName { get; set; } = string.Empty;
}
