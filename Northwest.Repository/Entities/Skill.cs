using Northwest.Persistence.Enums;

namespace Northwest.Persistence.Entities;

public class Skill
{
    //[Key]
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public Skills SkillType { get; set; }
}
