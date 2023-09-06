using System.Collections;
using System.Collections.Generic;
using SlimeBounce.Slime;
using UnityEngine;

namespace SlimeBounce.Player.Settings
{
    public interface IUpgradeSettings
    {
        public int GetUpgradeCost(UpgradeType type, int level);
        public List<float> GetUpgradeValues(UpgradeType type, int level);
        public bool IsUpgradePossible(UpgradeType type, int level);
        public int GetMaxLevel(UpgradeType type);
    }

    public enum UpgradeType
    {
        Lives,
        Slowdown,
        Investment,
        NewDropzone,
        FasterDropzone,
        AbilityShield,
        AbilityVoid,
        AbilityWave,
        AbilityFeast
    }
}