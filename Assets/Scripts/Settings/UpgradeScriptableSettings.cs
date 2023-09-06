using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Settings.Generic;
using SlimeBounce.Player;
using SlimeBounce.Player.Settings;

namespace SlimeBounce.Settings
{
    [CreateAssetMenu(fileName = "UpgradeSettings", menuName = "SlimeBounce/New Upgrade Settings", order = 10)]
    public class UpgradeScriptableSettings : ScriptableObject, IUpgradeSettings
    {
        [SerializeField] private Upgrade[] _upgrades;

        [Serializable]
        protected class Upgrade
        {
            public int MaxLevel;
            public List<float> ValuesPerLevel;
            public int BaseCost;
            public int CostPerLevel;
        }

        public int GetUpgradeCost(UpgradeType type, int level)
        {
            var upgrade = _upgrades[(int)type];
            return upgrade.CostPerLevel * level + upgrade.BaseCost;
        }

        public List<float> GetUpgradeValues(UpgradeType type, int level)
        {
            var upgrade = _upgrades[(int)type];
            List<float> targetValues = new List<float>();
            foreach (var value in upgrade.ValuesPerLevel)
            {
                targetValues.Add(value * level);
            }
            return targetValues;
        }

        public bool IsUpgradePossible(UpgradeType type, int level)
        {
            var upgrade = _upgrades[(int)type];
            return upgrade.MaxLevel > level;
        }

        public int GetMaxLevel(UpgradeType type)
        {
            return _upgrades[(int)type].MaxLevel;
        }
    }
}