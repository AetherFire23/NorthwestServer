using Northwest.Persistence.Enums;

namespace Northwest.Domain.Models;

public class UserGamePreparation
{
    public Guid UserId { get; set; }
    public RoleType RoleType { get; set; }
    public string Name { get; set; } = string.Empty;
}
