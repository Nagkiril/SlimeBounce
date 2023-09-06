using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Environment;
using SlimeBounce.UI.Count;
using SlimeBounce.Settings;
using SlimeBounce.Player.Settings;
using Zenject;

namespace SlimeBounce.Player
{
    public class UpgradeController : MonoBehaviour, IUpgradeDataProvider, IUpgradeActor
    {
        [Inject]
        private IUpgradeSettings _upgradeSettings;
        [Inject]
        private ICoinActor _coinActor;

        public bool PerformUpgrade(UpgradeType type)
        {
            bool upgradePerformed = false;
            var upgradeLevel = PlayerData.GetUpgradeLevel(type);
            if (upgradeLevel < GetMaxUpgradeLevel(type))
            {
                var cost = GetUpgradeCost(type);
                upgradePerformed = _coinActor.ChangeCoins(-1 * cost);
                if (upgradePerformed)
                {
                    PlayerData.SetUpgradeLevel(type, upgradeLevel + 1);
                }
            }
            return upgradePerformed;
        }

        public int GetUpgradeCost(UpgradeType type)
        {
            return _upgradeSettings.GetUpgradeCost(type, PlayerData.GetUpgradeLevel(type));
        }

        public List<float> GetUpgradeValues(UpgradeType type, bool getNextLevel = false)
        {
            return _upgradeSettings.GetUpgradeValues(type, PlayerData.GetUpgradeLevel(type) + (getNextLevel ? 1 : 0 ));
        }

        public int GetCurrentUpgradeLevel(UpgradeType type)
        {
            return PlayerData.GetUpgradeLevel(type);
        }

        public int GetMaxUpgradeLevel(UpgradeType type)
        {
            return _upgradeSettings.GetMaxLevel(type);
        }

        public bool IsUpgradePossible(UpgradeType type)
        {
            return _upgradeSettings.IsUpgradePossible(type, PlayerData.GetUpgradeLevel(type));
        }
    }
}