using SlimeBounce.Player.Settings;
using SlimeBounce.Slime;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.Settings
{
    [CreateAssetMenu(fileName = "PlayerLevelScriptableSettings", menuName = "SlimeBounce/New Player Level Settings", order = 10)]
    public class PlayerLevelScriptableSettings : ScriptableObject, IPlayerLevelSettings
    {
        [SerializeField] private PlayerLevelData[] _levelData;
        [SerializeField] private SlimeTypeUnlock[] _slimeUnlocks;

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

        private List<SlimeType> GetSlimeTypes(int playerLevel, bool includeEarlierUnlocks)
        {
            var unlockedSlimes = new List<SlimeType>();
            if (includeEarlierUnlocks)
                unlockedSlimes.Add(SlimeType.Regular);

            foreach (var progressStep in _slimeUnlocks)
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
            if (playerLevel >= _levelData.Length)
            {
                return _levelData[_levelData.Length - 1];
            }
            else
            {
                return _levelData[playerLevel];
            }
        }

        public float GetLevelUpExperience(int playerLevel)
        {
            return GetLevelData(playerLevel).LevelUpExp;

        }

        public float GetCurrencyMultiplier(int playerLevel) 
        {
            return GetLevelData(playerLevel).CurrencyMultiplier;
        }

        public float GetSpawnDelayReduction(int playerLevel)
        {
            return GetLevelData(playerLevel).SpawnDelayReduction;
        }

        public float GetSpeedMultiplier(int playerLevel)
        {
            return GetLevelData(playerLevel).SpeedMultiplier;
        }

        public List<SlimeType> GetUnlockedSlimes(int playerLevel)
        {
            return GetSlimeTypes(playerLevel, true);
        }

        public List<SlimeType> GetSlimeLevelUnlocks(int specificPlayerLevel)
        {
            return GetSlimeTypes(specificPlayerLevel, false);
        }
    }
}