namespace WebAPI.Entities;

public class Skill
{
    //[Key]
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public Enums.SkillEnum SkillType { get; set; }
}
