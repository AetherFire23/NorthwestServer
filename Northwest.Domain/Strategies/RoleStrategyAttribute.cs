using Northwest.Persistence.Enums;

namespace Northwest.Domain.Strategies;

[AttributeUsage(AttributeTargets.Class)]
public class RoleStrategyAttribute : Attribute
{
    public RoleType RoleType { get; set; }
    public RoleStrategyAttribute(RoleType role)
    {
        RoleType = role;
    }
}
