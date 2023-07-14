using Shared_Resources.Entities;

namespace WebAPI.UniversalSkills;

[UniversalSkill(Shared_Resources.Enums.SkillEnum.ShootLaser)]
public class ShootLaserFromEyes : IUniversalSkill
{
    private readonly PlayerContext _playerContext;
    public ShootLaserFromEyes(PlayerContext playerContext)
    {
        _playerContext = playerContext;
    }
    public async Task<bool> CanApplySkillEffect(Player player)
    {
        return true;
    }
    public async Task ApplyTickEffect(Player player)
    {
        int i = 0;
    }
}
