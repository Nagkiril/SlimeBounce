using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Settings.Generic;
using SlimeBounce.Player;

namespace SlimeBounce.Settings
{
    [CreateAssetMenu(fileName = "UpgradeSettings", menuName = "SlimeBounce/New Upgrade Settings", order = 10)]
    public class UpgradeSettings : GenericSettings<UpgradeSettings>
    {
        [SerializeField] protected Upgrade[] upgrades;

        [Serializable]
        protected class Upgrade
        {
            public int MaxLevel;
            public float ValuePerLevel;
            public int BaseCost;
            public int CostPerLevel;
        }


        private const string _loadPath = "Settings/UpgradeSettings";
        private static UpgradeSettings instance => (UpgradeSettings)GetInstance(_loadPath);

        public static int GetUpgradeCost(UpgradeType type, int level)
        {
            var upgrade = instance.upgrades[(int)type];
            return upgrade.CostPerLevel * level + upgrade.BaseCost;
        }

        public static float GetUpgradeValue(UpgradeType type, int level)
        {
            var upgrade = instance.upgrades[(int)type];
            return upgrade.ValuePerLevel * level;
        }

        public static bool IsUpgradePossible(UpgradeType type, int level)
        {
            var upgrade = instance.upgrades[(int)type];
            return upgrade.MaxLevel > level;
        }

        public static int GetMaxLevel(UpgradeType type)
        {
            return instance.upgrades[(int)type].MaxLevel;
        }
    }


    public enum UpgradeType
    {
        Lives,
        Slowdown,
        Investment,
        NewDropzone,
        FasterDropzone
    }
}