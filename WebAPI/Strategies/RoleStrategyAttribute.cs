using Shared_Resources.Enums;

namespace WebAPI.Strategies;

[AttributeUsage(AttributeTargets.Class)]
public class RoleStrategyAttribute : Attribute
{
    public RoleType RoleType { get; set; }
    public RoleStrategyAttribute(RoleType role)
    {
        RoleType = role;
    }
}
