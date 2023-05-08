using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Settings.Generic;
using SlimeBounce.Player;

namespace SlimeBounce.Settings
{
    [CreateAssetMenu(fileName = "LevelSettings", menuName = "SlimeBounce/New Level Settings", order = 10)]
    public class LevelSettings : GenericSettings<LevelSettings>
    {
        [SerializeField] protected int levelLivesDefault;
        [SerializeField] protected float rematchExpBonus;
        [SerializeField] protected float rematchExpMultiplier;
        [SerializeField] protected LevelData[] levels;

        [Serializable]
        protected class LevelData
        {
            public int CurrencyReward;
            public float WinExp;
            public SpawnSegment[] Spawns;
        }

        [Serializable]
        public class SpawnSegment
        {
            public SlimeType Slime;
            public int SpawnCount;

            public SpawnSegment(SlimeType type, int spawnCount)
            {
                Slime = type;
                SpawnCount = spawnCount;
            }
        }

        private const string _loadPath = "Settings/LevelSettings";
        private static LevelSettings instance => (LevelSettings)GetInstance(_loadPath);


        public static int GetDefaultLives()
        {
            return instance.levelLivesDefault;
        }

        public static int GetLevelReward()
        {
            return GetAppliedLevel().CurrencyReward;
        }

        public static int GetLevelsAmount()
        {
            return instance.levels.Length;
        }

        public static List<SpawnSegment> GetOrderedSpawns()
        {
            return GetAppliedLevel().Spawns.Select(x => x = new SpawnSegment(x.Slime, x.SpawnCount)).ToList();
        }

        public static float GetRewardExp()
        {
            int overlevels = PlayerData.PassedLevels - instance.levels.Length;
            if (overlevels > 0)
            {
                return instance.levels[instance.levels.Length - 1].WinExp * (1 + overlevels * instance.rematchExpMultiplier) + instance.rematchExpBonus;
            }
            else
            {
                return GetAppliedLevel().WinExp;
            }
        }

        private static LevelData GetAppliedLevel()
        {
            var appliedLevel = instance.levels[PlayerData.CurrentLevel];
            return appliedLevel;
        }
    }


    public enum SlimeType
    {
        Regular,
        Charger,
        Protector,
        Explosive,
        Pop,
        Waver,
        Flamer,
        Golden
    }
}