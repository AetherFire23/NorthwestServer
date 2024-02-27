namespace WebAPI.Models;

public class FriendNotificationProperties
{
    public string InviterUserName { get; set; }
    public string InvitedUserName { get; set; }
    // sera accepted ou non dans le controller
}
