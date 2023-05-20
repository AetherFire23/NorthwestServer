using Shared_Resources.Entities;

namespace WebAPI.UniversalSkills
{
    public interface IUniversalSkill
    {
        Task<bool> CanApplySkillEffect(Player player);
        Task ApplyTickEffect(Player player);
    }
}
