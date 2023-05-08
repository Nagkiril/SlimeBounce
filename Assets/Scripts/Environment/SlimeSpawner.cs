using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Settings;
using SlimeBounce.Slime;


namespace SlimeBounce.Environment
{
    public class SlimeSpawner : MonoBehaviour
    {
        public event Action<SlimeCore> OnSlimeSpawned;
        float _untilNextSpawn;
        float _slimeSpeedMultiplier;
        float _spawnDelayReduction;
        List<LevelSettings.SpawnSegment> _slimesToSpawn;

        public int SlimesTotal { get; private set; }
        public int SlimesUnspawned { get; private set; }

        

        public void StartSpawning(List<SlimeType> allowedSlimes, float slimeSpeedMultiplier, float spawnDelayReduction)
        {
            if (_untilNextSpawn <= 0)
            {
                _untilNextSpawn = 0.01f;
                var _unlockedSlimeTypes = allowedSlimes;
                _slimeSpeedMultiplier = slimeSpeedMultiplier;
                _spawnDelayReduction = spawnDelayReduction;
                _slimesToSpawn = LevelSettings.GetOrderedSpawns();

                for (var i = _slimesToSpawn.Count - 1; i >= 0; i--)
                {
                    if (!_unlockedSlimeTypes.Contains(_slimesToSpawn[i].Slime))
                    {
                        _slimesToSpawn.RemoveAt(i);
                    }
                }

                SlimesTotal = 0;
                foreach (var spawn in _slimesToSpawn)
                {
                    SlimesTotal += spawn.SpawnCount;
                }
                SlimesUnspawned = SlimesTotal;
            }
        }

        public void StopSpawning()
        {
            SlimesTotal = -1;
            _untilNextSpawn = -1;
        }

        // Update is called once per frame
        void Update()
        {
            if (_untilNextSpawn > 0)
            {
                _untilNextSpawn -= Time.deltaTime;
                if (_untilNextSpawn <= 0f)
                {
                    _untilNextSpawn = SpawnSettings.GetSpawnDelay() - _spawnDelayReduction;
                    SpawnSlime();
                }
            }
        }

        void SpawnSlime()
        {
            if (SlimesUnspawned > 0)
            {
                var newSlime = Instantiate(SpawnSettings.GetSpawnPrefab(GetNextSlimeType()), GetSpawnPosition(), transform.rotation).GetComponent<SlimeCore>();
                newSlime.AdjustBaseSpeed(_slimeSpeedMultiplier);
                SlimesUnspawned--;
                OnSlimeSpawned?.Invoke(newSlime);
            }
        }

        private SlimeType GetNextSlimeType()
        {
            var nextSlimeType = SlimeType.Regular;
            if (SlimesUnspawned > 0)
            {
                var spawnedSlimeIndex = SlimesTotal - SlimesUnspawned;
                for (var i = 0; i < _slimesToSpawn.Count; i++)
                {
                    spawnedSlimeIndex -= _slimesToSpawn[i].SpawnCount;
                    if (spawnedSlimeIndex < 0)
                        return _slimesToSpawn[i].Slime;
                }
            }
            return nextSlimeType;
        }

        Vector3 GetSpawnPosition()
        {
            return transform.position + transform.right * UnityEngine.Random.Range(-1f, 1f) * transform.localScale.x * 0.5f;
        }
    }
}