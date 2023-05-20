﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared_Resources.GameTasks
{
    public enum GameTaskCodes
    {
        // there 3 exist in QuarterDeck AND Forecastle
        ChargeCannon,
        FireCannon,
        RepairCannon,

        // These 3 4 appear in mizzenmast, mainmast, foremast
        RaiseSail,
        LowerSail,
        SetSail,
        RepairSail,
        //

        Watch,
        ChargeMortar,
        FireMortar,
        RepairMortar,

        Expedition,
        Evacuation,

        Fish,
        Fire,

        RaiseAnchor,
        LowererAnchor,
        CookMeat,
        CookFish,
        CookStew,

        Lock,
        Unlock,
    }
}
