using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SlimeBounce.Environment.DropoutComponents
{
    public class DropoutSpawner : MonoBehaviour
    {
        [SerializeField] private DropoutZone _dropoutPrefab;
        [SerializeField] private DropoutIndicator _zoneIndicator;
        [SerializeField] private GameObject _visualIndicator;
        private DropoutZone _spawnedZone;
        [Inject]
        private DropoutZone.Factory _dropoutFactory;

        public Action<DropoutSpawner> OnSlimeConsumed;
        public bool IsFree => _spawnedZone == null;


        private void Awake()
        {
            _visualIndicator.SetActive(false);
        }

        private void OnSlimeConsume()
        {
            OnSlimeConsumed?.Invoke(this);
        }

        public bool SpawnZone(DropoutZoneData spawnerData)
        {
            spawnerData.SpawnedIndicator = _zoneIndicator;
            if (IsFree)
            {
                _spawnedZone = _dropoutFactory.Create(_dropoutPrefab);
                _spawnedZone.transform.SetParent(transform, false);
                _spawnedZone.Initialize(spawnerData);
                _spawnedZone.OnSlimeConsumed += OnSlimeConsume;
                return true;
            } else
            {
                _spawnedZone.Initialize(spawnerData);
                return false;
            }
        }

        public void HideSpawnedZone()
        {
            if (_spawnedZone != null)
            {
                _spawnedZone.Hide();
            }
        }

        public void MultiplyCooldown(float multiplier)
        {
            if (_spawnedZone != null)
            {
                _spawnedZone.MultiplyCooldown(multiplier);
            }
        }

        public void ResetCooldown()
        {
            if (_spawnedZone != null)
            {
                _spawnedZone.ResetCooldown();
            }
        }
    }
}