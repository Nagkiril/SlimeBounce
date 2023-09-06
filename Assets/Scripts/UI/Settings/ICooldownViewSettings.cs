using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.UI.Abilities.CooldownComponents;

namespace SlimeBounce.UI.Settings
{
    public interface ICooldownViewSettings
    {
        public SegmentedCooldownDial GetDialPrefab(int segmentCount);
    }
}