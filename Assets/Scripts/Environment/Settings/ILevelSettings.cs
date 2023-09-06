using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Slime;


namespace SlimeBounce.Environment.Settings
{
    public interface ILevelSettings
    {
        public int GetDefaultLives();

        public int GetLevelReward();

        public int GetLevelsAmount();

        public List<SpawnSegment> GetOrderedSpawns();

        public float GetRewardExp();
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
}