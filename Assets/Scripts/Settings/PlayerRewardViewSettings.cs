using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Settings.Generic;
using SlimeBounce.Slime;

namespace SlimeBounce.Settings
{
    [CreateAssetMenu(fileName = "PlayerRewardViewSettings", menuName = "SlimeBounce/New Player Reward View Settings", order = 10)]
    public class PlayerRewardViewSettings : GenericSettings<PlayerRewardViewSettings>
    {
        [SerializeField] RewardViewData[] slimeData;
        [SerializeField] RewardViewData currencyData;
        [SerializeField] RewardViewData speedData;
        [SerializeField] RewardViewData spawnData;


        private const string _loadPath = "Settings/PlayerRewardViewSettings";
        private static PlayerRewardViewSettings instance => (PlayerRewardViewSettings)GetInstance(_loadPath);

        public static RewardViewData GetSlimeReward(SlimeType slimeType)
        {
            return instance.slimeData[(int)slimeType];
        }

        public static RewardViewData GetCurrencyReward()
        {
            return instance.currencyData.Clone();
        }

        public static RewardViewData GetSpeedReward()
        {
            return instance.speedData.Clone();
        }

        public static RewardViewData GetSpawnReward()
        {
            return instance.spawnData.Clone();
        }
    }

    [Serializable]
    public class RewardViewData
    {
        public GameObject Header;
        public string Name;
        public string Description;

        public RewardViewData Clone()
        {
            var newData = new RewardViewData();
            newData.Header = Header;
            newData.Name = Name;
            newData.Description = Description;

            return newData;
        }
    }
}