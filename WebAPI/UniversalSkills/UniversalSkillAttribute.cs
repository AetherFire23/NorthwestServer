using WebAPI.Enums;

namespace WebAPI.UniversalSkills;

[AttributeUsage(AttributeTargets.Class)]
public class UniversalSkillAttribute : Attribute
{
    public SkillEnum SkillAttribute { get; set; }

    public UniversalSkillAttribute(SkillEnum skill)
    {
        SkillAttribute = skill;
    }
}
