using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Environment.Settings;
using SlimeBounce.Settings;
using SlimeBounce.Slime;
using Zenject;


namespace SlimeBounce.Environment
{
    public class SlimeSpawner : MonoBehaviour
    {
        private float _untilNextSpawn;
        private float _slimeSpeedMultiplier;
        private float _spawnDelayReduction;
        private List<SpawnSegment> _slimesToSpawn;

        public event Action<SlimeCore> OnSlimeSpawned;
        public int SlimesTotal { get; private set; }
        public int SlimesUnspawned { get; private set; }

        [Inject]
        private SlimeCore.Factory _slimeFactory;
        [Inject]
        private ILevelSettings _levelSettings;
        [Inject]
        private ISpawnSettings _spawnSettings;

        private void Update()
        {
            if (_untilNextSpawn > 0)
            {
                _untilNextSpawn -= Time.deltaTime;
                if (_untilNextSpawn <= 0f)
                {
                    _untilNextSpawn = _spawnSettings.GetSpawnDelay() - _spawnDelayReduction;
                    SpawnSlime();
                }
            }
        }

        private void SpawnSlime()
        {
            if (SlimesUnspawned > 0)
            {
                var newSlime = _slimeFactory.Create(_spawnSettings.GetSpawnPrefab(GetNextSlimeType()));
                newSlime.transform.position = GetSpawnPosition();
                newSlime.transform.rotation = transform.rotation;
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

        private Vector3 GetSpawnPosition()
        {
            return transform.position + transform.right * UnityEngine.Random.Range(-1f, 1f) * transform.localScale.x * 0.5f;
        }

        public void StartSpawning(List<SlimeType> allowedSlimes, float slimeSpeedMultiplier, float spawnDelayReduction)
        {
            if (_untilNextSpawn <= 0)
            {
                _untilNextSpawn = 0.01f;
                var _unlockedSlimeTypes = allowedSlimes;
                _slimeSpeedMultiplier = slimeSpeedMultiplier;
                _spawnDelayReduction = spawnDelayReduction;
                _slimesToSpawn = _levelSettings.GetOrderedSpawns();

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
    }
}