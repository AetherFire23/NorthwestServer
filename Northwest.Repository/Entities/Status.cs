using Northwest.Persistence.Enums;

namespace Northwest.Persistence.Entities;

public class Status
{
    public Guid Id { get; set; }
    public StatusEffects Effect { get; set; }
    public string SerializedProperties = string.Empty;
}
