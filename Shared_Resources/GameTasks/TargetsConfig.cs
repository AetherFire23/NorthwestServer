using Shared_Resources.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared_Resources.GameTasks
{
    public class TargetsConfig
    {
        public GameTaskCodes TaskCode { get; set; } = GameTaskCodes.Cook;
        public PlayerTargetRequirement PlayerTarget { get; set; } = PlayerTargetRequirement.None;
        public RoomTargetRequirement RoomTarget { get; set; } = RoomTargetRequirement.None;
        public SpecialDialogRequirement SpecialDialog { get; set; } = SpecialDialogRequirement.None;
        bool RequiresTarget => PlayerTarget != PlayerTargetRequirement.None && RoomTarget != RoomTargetRequirement.None;

        // 

        //public TargetsConfig ConfigurePlayerTargets(PlayerTargetRequirement targetType, int targetCount)
        //{


        //}
    }
}
