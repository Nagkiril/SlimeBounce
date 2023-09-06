using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Abilities;
using SlimeBounce.Slime;
using SlimeBounce.Environment;
using Zenject;

namespace SlimeBounce.Slime.Loot
{
    public class SlimeLootDrops : MonoBehaviour
    {
        [SerializeField] private PickableLoot[] _guaranteedLoot;
        [SerializeField] private float _randomDropChance;
        [SerializeField] private RandomLootEntry[] _randomLoot;
        [SerializeField] private SlimeCore _targetCore;

        private const float WIDTH_DISPERSION = 3f;
        private const float LENGTH_DISPERSION = 3f;

        [Inject]
        private ILootEnvironmentProvider _lootEnvironment;
        [Inject]
        private PickableLoot.Factory _lootFactory;
        [Inject]
        private ILevelStateProvider _levelState;

        [Serializable]
        private class RandomLootEntry
        {
            public PickableLoot LootPrefab;
            public float ApplyChance;
            public bool IsFinalLoot;
        }

        private bool _isLootDispensed;

        private void Awake()
        {
            _targetCore.OnSlimeDestroyed += OnSlimeDestroyed;
            _targetCore.OnSlimeConsumed += OnSlimeConsumed;
            _targetCore.OnSlimeConsumed += OnSlimeEscaped;
        }

        private void OnSlimeDestroyed()
        {
            if (!_isLootDispensed && _levelState.IsLevelInProgress)
            {
                DispenseGuaranteedLoot(false);
                DispenseRandomLoot();
                _isLootDispensed = true;
            }
        }

        private void OnSlimeEscaped()
        {
            _isLootDispensed = true;
        }

        private void OnSlimeConsumed()
        {
            if (!_isLootDispensed)
            {
                _isLootDispensed = true;
                DispenseGuaranteedLoot(true);
            }
        }

        private void DispenseGuaranteedLoot(bool autoPickup)
        {
            foreach (var loot in _guaranteedLoot)
            {
                SpawnLoot(loot, autoPickup);
            }
        }

        private void DispenseRandomLoot()
        {
            if (UnityEngine.Random.value <= _randomDropChance)
            {
                foreach (var entry in _randomLoot)
                {
                    if (UnityEngine.Random.value <= entry.ApplyChance)
                    {
                        SpawnLoot(entry.LootPrefab);
                        if (entry.IsFinalLoot)
                            break;
                    }
                }
            }
        }

        private void SpawnLoot(PickableLoot loot, bool autopickup = false)
        {
            if (loot.CheckSpawnAllowed(_lootEnvironment.GetEnvironment()))
            {
                var newLoot = _lootFactory.Create(loot);
                newLoot.transform.position = transform.position;
                newLoot.transform.rotation = transform.rotation;
                if (autopickup)
                {
                    newLoot.Pickup();
                }
                else
                {
                    Vector3 dispersePosition = newLoot.transform.position;
                    dispersePosition += transform.right * WIDTH_DISPERSION * UnityEngine.Random.Range(-1f, 1f);
                    dispersePosition += transform.forward * LENGTH_DISPERSION * UnityEngine.Random.Range(-1f, 1f);
                    newLoot.DisperseToPosition(dispersePosition);
                }
            }
        }
    }
}