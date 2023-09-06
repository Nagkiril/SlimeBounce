using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SlimeBounce.Settings.Generic;
using SlimeBounce.UI.Abilities.CooldownComponents;
using SlimeBounce.UI.Settings;

namespace SlimeBounce.Settings
{
    [CreateAssetMenu(fileName = "CooldownViewSettings", menuName = "SlimeBounce/New Cooldown View Settings", order = 10)]
    public class CooldownViewScriptableSettings : ScriptableObject, ICooldownViewSettings
    {
        [SerializeField] private DialEntry[] _dials;

        [Serializable]
        private class DialEntry
        {
            public int SegmentCount;
            public SegmentedCooldownDial CooldownPrefab;
        }

        public SegmentedCooldownDial GetDialPrefab(int segmentCount)
        {
            var entry = _dials.First(x => x.SegmentCount == segmentCount);
            SegmentedCooldownDial targetDial = null;
            if (entry != null)
            {
                targetDial = entry.CooldownPrefab;
            }
            return targetDial;
        }
    }
}