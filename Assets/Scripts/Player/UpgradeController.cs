using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Environment;
using SlimeBounce.UI.Count;
using SlimeBounce.Settings;

namespace SlimeBounce.Player
{
    public class UpgradeController : MonoBehaviour
    {
        static UpgradeController _instance;

        public static event Action<UpgradeType> OnUpgradePerformed;


        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            } else
            {
                Debug.LogWarning($"There's at least two Upgrade Controllers, one of the controllers is in {gameObject.name}");
                Destroy(this);
            }
        }

        public static bool PerformUpgrade(UpgradeType type)
        {
            bool upgradePerformed = false;
            var upgradeLevel = PlayerData.GetUpgradeLevel(type);
            if (upgradeLevel < GetMaxUpgradeLevel(type))
            {
                var cost = GetUpgradeCost(type);
                upgradePerformed = CoinController.ChangeCoins(-1 * cost);
                if (upgradePerformed)
                {

                    PlayerData.SetUpgradeLevel(type, upgradeLevel + 1);
                    OnUpgradePerformed?.Invoke(type);
                }
            }
            return upgradePerformed;
        }


        public static int GetUpgradeCost(UpgradeType type)
        {
            return UpgradeSettings.GetUpgradeCost(type, PlayerData.GetUpgradeLevel(type));
        }

        public static float GetUpgradeValue(UpgradeType type, bool getNextLevel = false)
        {
            return UpgradeSettings.GetUpgradeValue(type, PlayerData.GetUpgradeLevel(type) + (getNextLevel ? 1 : 0 ));
        }

        public static int GetCurrentUpgradeLevel(UpgradeType type)
        {
            return PlayerData.GetUpgradeLevel(type);
        }

        public static int GetMaxUpgradeLevel(UpgradeType type)
        {
            return UpgradeSettings.GetMaxLevel(type);
        }

        public static bool IsUpgradePossible(UpgradeType type)
        {
            return UpgradeSettings.IsUpgradePossible(type, PlayerData.GetUpgradeLevel(type));
        }
    }
}