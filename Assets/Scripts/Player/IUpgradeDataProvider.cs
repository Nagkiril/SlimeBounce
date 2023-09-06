using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Player.Settings;

namespace SlimeBounce.Player
{
    public interface IUpgradeDataProvider
    {
        public int GetUpgradeCost(UpgradeType type);

        public List<float> GetUpgradeValues(UpgradeType type, bool getNextLevel = false);

        public int GetCurrentUpgradeLevel(UpgradeType type);

        public int GetMaxUpgradeLevel(UpgradeType type);

        public bool IsUpgradePossible(UpgradeType type);
    }
}