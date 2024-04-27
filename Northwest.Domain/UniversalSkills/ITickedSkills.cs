using Northwest.Persistence.Entities;

namespace Northwest.Domain.UniversalSkills;

public interface ITickedSkills
{
    Task<bool> CanApplySkillEffect(Player player);
    Task ApplyTickEffect(Player player);
}
