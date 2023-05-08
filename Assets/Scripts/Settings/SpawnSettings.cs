using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Settings.Generic;
using SlimeBounce.Slime;

namespace SlimeBounce.Settings
{
    [CreateAssetMenu(fileName = "SpawnSettings", menuName = "SlimeBounce/New Spawn Settings", order = 10)]
    public class SpawnSettings : GenericSettings<SpawnSettings>
    {
        [SerializeField] protected float spawnDelay;
        [SerializeField] SlimeCore[] spawnPrefabs;

        private const string _loadPath = "Settings/SpawnSettings";
        private static SpawnSettings instance => (SpawnSettings)GetInstance(_loadPath);

        public static float GetSpawnDelay()
        {
            return instance.spawnDelay;
        }

        public static SlimeCore GetSpawnPrefab(SlimeType slimeType)
        {
            return instance.spawnPrefabs[(int)slimeType];
        }
    }
}