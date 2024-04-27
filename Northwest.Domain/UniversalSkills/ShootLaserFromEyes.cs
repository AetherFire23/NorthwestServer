using Northwest.Persistence;
using Northwest.Persistence.Entities;
using Northwest.Persistence.Enums;

namespace Northwest.Domain.UniversalSkills;

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
