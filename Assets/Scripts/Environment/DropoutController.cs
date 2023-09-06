using SlimeBounce.Environment.DropoutComponents;
using SlimeBounce.Player;
using SlimeBounce.Player.Settings;
using System;
using UnityEngine;
using Zenject;
using SlimeBounce.Environment.Settings;

namespace SlimeBounce.Environment
{
    public class DropoutController : MonoBehaviour, IDropoutCooldownManager
    {
        [SerializeField] private DropoutSpawner[] _dropoutSpawns;

        public event Action OnCooldownStarted;

        [Inject]
        private IDropoutSettings _settings;
        [Inject]
        private IUpgradeDataProvider _upgradeData;
        [Inject]
        private ILevelStateProvider _levelState;

        private void Start()
        {
            CheckZoneSpawns();
            _levelState.OnLevelStarted += OnLevelStart;
            _levelState.OnLevelEnded += OnLevelEnd;
        }

        private void OnDestroy()
        {
            _levelState.OnLevelEnded -= OnLevelEnd;
            _levelState.OnLevelStarted -= OnLevelStart;
        }

        private void CheckZoneSpawns()
        {
            DropoutZoneData spawnerData = new DropoutZoneData();
            spawnerData.Cooldown = _settings.DropoutBaseCooldown * (1f - 0.01f * _upgradeData.GetUpgradeValues(UpgradeType.FasterDropzone)[0]);
            int dropoutZoneCount = _settings.DropoutBaseAmount + (int)_upgradeData.GetUpgradeValues(UpgradeType.NewDropzone)[0];
            for (var i = 0; i < dropoutZoneCount; i++)
            {
                if (_dropoutSpawns[i].SpawnZone(spawnerData))
                    _dropoutSpawns[i].OnSlimeConsumed += OnSpawnerConsumption;
            }
        }

        private void OnLevelStart()
        {
            CheckZoneSpawns();
        }

        private void OnLevelEnd(bool isWin)
        {
            foreach (var zone in _dropoutSpawns)
            {
                zone.HideSpawnedZone();
            }
        }

        private void OnSpawnerConsumption(DropoutSpawner consumer)
        {
            OnCooldownStarted?.Invoke();
        }

        public void AddCooldownMultiplier(float multiplier)
        {
            foreach (var dropout in _dropoutSpawns)
            {
                dropout.MultiplyCooldown(multiplier);
            }
        }

        public void ResetCooldowns()
        {
            foreach (var dropout in _dropoutSpawns)
            {
                dropout.ResetCooldown();
            }
        }
    }
}