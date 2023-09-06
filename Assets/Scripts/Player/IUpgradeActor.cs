using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Player.Settings;

namespace SlimeBounce.Player
{
    public interface IUpgradeActor
    {
        public bool PerformUpgrade(UpgradeType type);
    }
}