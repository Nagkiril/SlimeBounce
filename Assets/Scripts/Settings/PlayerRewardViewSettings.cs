using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.UI.Settings;
using SlimeBounce.Settings.Generic;
using SlimeBounce.Slime;

namespace SlimeBounce.Settings
{
    [CreateAssetMenu(fileName = "PlayerRewardViewSettings", menuName = "SlimeBounce/New Player Reward View Settings", order = 10)]
    public class PlayerRewardViewSettings : ScriptableObject, IPlayerRewardViewSettings
    {
        [SerializeField] private RewardViewData[] _slimeData;
        [SerializeField] private RewardViewData _currencyData;
        [SerializeField] private RewardViewData _speedData;
        [SerializeField] private RewardViewData _spawnData;


        public RewardViewData GetSlimeReward(SlimeType slimeType)
        {
            return _slimeData[(int)slimeType];
        }

        public RewardViewData GetCurrencyReward()
        {
            return _currencyData.Clone();
        }

        public RewardViewData GetSpeedReward()
        {
            return _speedData.Clone();
        }

        public RewardViewData GetSpawnReward()
        {
            return _spawnData.Clone();
        }
    }

}