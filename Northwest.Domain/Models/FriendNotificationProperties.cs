namespace Northwest.Domain.Models;

public class FriendNotificationProperties
{
    public string InviterUserName { get; set; } = string.Empty;
    public string InvitedUserName { get; set; } = string.Empty;
    // sera accepted ou non dans le controller
}
