using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Settings.Generic;
using SlimeBounce.Slime;

namespace SlimeBounce.Settings
{
    [CreateAssetMenu(fileName = "PlayerLevelSettings", menuName = "SlimeBounce/New Player Level Settings", order = 10)]
    public class PlayerLevelSettings : GenericSettings<PlayerLevelSettings>
    {
        [SerializeField] PlayerLevelData[] levelData;
        [SerializeField] SlimeTypeUnlock[] slimeUnlocks;


        //These values could potentially be parsed out of an external source, rather than set up in the inspector manually
        [Serializable]
        private class PlayerLevelData
        {
            public float LevelUpExp;
            public float CurrencyMultiplier;
            public float SpeedMultiplier;
            public float SpawnDelayReduction;
        }

        [Serializable]
        private class SlimeTypeUnlock
        {
            public int RequiredLevel;
            public SlimeType NewType;
        }


        private const string _loadPath = "Settings/PlayerLevelSettings";
        private static PlayerLevelSettings instance => (PlayerLevelSettings)GetInstance(_loadPath);

        public static float GetLevelUpExperience(int playerLevel)
        {
            return instance.GetLevelData(playerLevel).LevelUpExp;

        }

        public static float GetCurrencyMultiplier(int playerLevel) 
        {
            return instance.GetLevelData(playerLevel).CurrencyMultiplier;
        }

        public static float GetSpawnDelayReduction(int playerLevel)
        {
            return instance.GetLevelData(playerLevel).SpawnDelayReduction;
        }

        public static float GetSpeedMultiplier(int playerLevel)
        {
            return instance.GetLevelData(playerLevel).SpeedMultiplier;
        }

        public static List<SlimeType> GetUnlockedSlimes(int playerLevel)
        {
            return instance.GetSlimeTypes(playerLevel, true);
        }

        public static List<SlimeType> GetSlimeLevelUnlocks(int specificPlayerLevel)
        {
            return instance.GetSlimeTypes(specificPlayerLevel, false);
        }

        private List<SlimeType> GetSlimeTypes(int playerLevel, bool includeEarlierUnlocks)
        {
            var unlockedSlimes = new List<SlimeType>();
            if (includeEarlierUnlocks)
                unlockedSlimes.Add(SlimeType.Regular);

            foreach (var progressStep in instance.slimeUnlocks)
            {
                if (includeEarlierUnlocks ? (progressStep.RequiredLevel <= playerLevel) : (progressStep.RequiredLevel == playerLevel))
                {
                    unlockedSlimes.Add(progressStep.NewType);
                }
            }

            return unlockedSlimes;
        }

        private PlayerLevelData GetLevelData(int playerLevel)
        {
            if (playerLevel >= levelData.Length)
            {
                return levelData[levelData.Length - 1];
            }
            else
            {
                return levelData[playerLevel];
            }
        }
    }
}