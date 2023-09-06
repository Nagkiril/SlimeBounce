using System.Collections;
using System.Collections.Generic;
using SlimeBounce.Slime;
using UnityEngine;

namespace SlimeBounce.Player.Settings
{
    public interface IPlayerLevelSettings
    {
        public float GetLevelUpExperience(int playerLevel);
        public float GetCurrencyMultiplier(int playerLevel);
        public float GetSpawnDelayReduction(int playerLevel);
        public float GetSpeedMultiplier(int playerLevel);
        public List<SlimeType> GetUnlockedSlimes(int playerLevel);
        public List<SlimeType> GetSlimeLevelUnlocks(int specificPlayerLevel);
    }
}