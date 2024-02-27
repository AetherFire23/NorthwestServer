using WebAPI.Entities;

namespace WebAPI.UniversalSkills;

public interface ITickedSkills
{
    Task<bool> CanApplySkillEffect(Player player);
    Task ApplyTickEffect(Player player);
}
