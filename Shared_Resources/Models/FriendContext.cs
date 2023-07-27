using System;

namespace Shared_Resources.Models;

public class FriendContext
{
    public Guid CallerUserId { get; set; }
    public string CallerUserName { get; set; } = string.Empty;
    public string TargetName { get; set; } = string.Empty;
    public Guid TargetId { get; set; }
    public bool Accepted { get; set; }
}
