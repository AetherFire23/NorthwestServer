using WebAPI.Enums;

namespace WebAPI.Models;

public class UserGamePreparation
{
    public Guid UserId { get; set; }
    public RoleType RoleType { get; set; }
    public string Name { get; set; } = string.Empty;
}
