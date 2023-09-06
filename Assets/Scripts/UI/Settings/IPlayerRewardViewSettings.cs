using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Slime;
using SlimeBounce.UI.Abilities.CooldownComponents;

namespace SlimeBounce.UI.Settings
{
    public interface IPlayerRewardViewSettings
    {
        public RewardViewData GetSlimeReward(SlimeType slimeType);
        public RewardViewData GetCurrencyReward();
        public RewardViewData GetSpeedReward();
        public RewardViewData GetSpawnReward();
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