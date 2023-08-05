﻿using Shared_Resources.Entities;
using Shared_Resources.Enums;

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
