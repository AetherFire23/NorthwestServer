using WebAPI.Enums;

namespace WebAPI.Entities;

public class Status
{
    public Guid Id { get; set; }
    public StatusEffect Effect { get; set; }
    public string SerializedProperties = string.Empty;
}
