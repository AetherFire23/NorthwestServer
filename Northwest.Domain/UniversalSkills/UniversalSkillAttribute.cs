using Northwest.Persistence.Enums;

namespace Northwest.Domain.UniversalSkills;

[AttributeUsage(AttributeTargets.Class)]
public class UniversalSkillAttribute : Attribute
{
    public Skills SkillAttribute { get; set; }

    public UniversalSkillAttribute(Skills skill)
    {
        SkillAttribute = skill;
    }
}
