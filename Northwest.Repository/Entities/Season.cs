using Northwest.Persistence.Enums;

namespace Northwest.Persistence.Entities;

public class Season
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public Seasons Current { get; set; }
    public bool Frozen { get; set; }// Degres de gel ?
}
