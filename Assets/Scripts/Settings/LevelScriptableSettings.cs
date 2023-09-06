using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Settings.Generic;
using SlimeBounce.Player;
using SlimeBounce.Environment.Settings;

namespace SlimeBounce.Settings
{
    [CreateAssetMenu(fileName = "LevelScriptableSettings", menuName = "SlimeBounce/New Level Settings", order = 10)]
    public class LevelScriptableSettings : GenericSettings<LevelScriptableSettings>, ILevelSettings
    {
        [SerializeField] private int _levelLivesDefault;
        [SerializeField] private float _rematchExpBonus;
        [SerializeField] private float _rematchExpMultiplier;
        [SerializeField] private LevelData[] _levels;

        [Serializable]
        protected class LevelData
        {
            public int CurrencyReward;
            public float WinExp;
            public SpawnSegment[] Spawns;
        }

        private LevelData GetAppliedLevel()
        {
            var appliedLevel = _levels[PlayerData.CurrentLevel];
            return appliedLevel;
        }

        public int GetDefaultLives()
        {
            return _levelLivesDefault;
        }

        public int GetLevelReward()
        {
            return GetAppliedLevel().CurrencyReward;
        }

        public int GetLevelsAmount()
        {
            return _levels.Length;
        }

        public List<SpawnSegment> GetOrderedSpawns()
        {
            return GetAppliedLevel().Spawns.Select(x => x = new SpawnSegment(x.Slime, x.SpawnCount)).ToList();
        }

        public float GetRewardExp()
        {
            int overlevels = PlayerData.PassedLevels - _levels.Length;
            if (overlevels > 0)
            {
                return _levels[_levels.Length - 1].WinExp * (1 + overlevels * _rematchExpMultiplier) + _rematchExpBonus;
            }
            else
            {
                return GetAppliedLevel().WinExp;
            }
        }
    }
}