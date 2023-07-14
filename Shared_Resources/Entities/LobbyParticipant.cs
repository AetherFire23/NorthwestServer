
using System;

namespace Shared_Resources.Entities;

public class LobbyParticipant
{
    //[Key]
    public Guid Id { get; set; }
    public Guid LobbyId { get; set; }
    public Guid ParticipantId { get; set; }
}
