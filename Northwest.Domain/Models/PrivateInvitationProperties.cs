namespace Northwest.Domain.Models;

public class PrivateInvitationProperties
{
    public Guid FromPlayerId { get; set; }
    public Guid RoomId { get; set; }
    public string FromPlayerName { get; set; } = string.Empty;
    public string ToPlayerName { get; set; } = string.Empty;
    public bool IsAccepted { get; set; }
}
