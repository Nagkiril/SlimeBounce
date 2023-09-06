using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Environment.Settings;
using SlimeBounce.Settings.Generic;
using SlimeBounce.Slime;

namespace SlimeBounce.Settings
{
    [CreateAssetMenu(fileName = "SpawnSettings", menuName = "SlimeBounce/New Spawn Settings", order = 10)]
    public class SpawnScriptableSettings : ScriptableObject, ISpawnSettings
    {
        [SerializeField] private float _spawnDelay;
        [SerializeField] private SlimeCore[] _spawnPrefabs;


        public float GetSpawnDelay()
        {
            return _spawnDelay;
        }

        public SlimeCore GetSpawnPrefab(SlimeType slimeType)
        {
            return _spawnPrefabs[(int)slimeType];
        }
    }
}