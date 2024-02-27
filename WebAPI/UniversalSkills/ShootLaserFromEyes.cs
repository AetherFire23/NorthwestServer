using WebAPI.Entities;
using WebAPI.Enums;

namespace WebAPI.UniversalSkills;

[UniversalSkill(SkillEnum.ShootLaser)]
public class ShootLaserFromEyes : ITickedSkills
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
    }
}
