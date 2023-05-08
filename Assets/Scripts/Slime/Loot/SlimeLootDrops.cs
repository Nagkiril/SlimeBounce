using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Slime;


namespace SlimeBounce.Slime.Loot
{
    public class SlimeLootDrops : MonoBehaviour
    {
        [SerializeField] PickableLoot[] guaranteedLoot;
        [SerializeField] float randomDropChance;
        [SerializeField] RandomLootEntry[] randomLoot;
        [SerializeField] SlimeCore targetCore;

        const float WIDTH_DISPERSION = 3f;
        const float LENGTH_DISPERSION = 3f;

        [Serializable]
        private class RandomLootEntry
        {
            public PickableLoot LootPrefab;
            public float ApplyChance;
            public bool IsFinalLoot;
        }

        bool _isLootDispensed;

        // Start is called before the first frame update
        void Awake()
        {
            targetCore.OnSlimeDestroyed += OnSlimeDestroyed;
            targetCore.OnSlimeConsumed += OnSlimeConsumed;
            targetCore.OnSlimeConsumed += OnSlimeEscaped;
        }

        void OnSlimeDestroyed()
        {
            if (!_isLootDispensed)
            {
                DispenseGuaranteedLoot(false);
                DispenseRandomLoot();
                _isLootDispensed = true;
            }
        }

        void OnSlimeEscaped()
        {
            _isLootDispensed = true;
        }

        void OnSlimeConsumed()
        {
            if (!_isLootDispensed)
            {
                _isLootDispensed = true;
                DispenseGuaranteedLoot(true);
            }
        }

        void DispenseGuaranteedLoot(bool autoPickup)
        {
            foreach (var loot in guaranteedLoot)
            {
                SpawnLoot(loot, autoPickup);
            }
        }

        void DispenseRandomLoot()
        {
            if (UnityEngine.Random.value <= randomDropChance)
            {
                foreach (var entry in randomLoot)
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

        void SpawnLoot(PickableLoot loot, bool autopickup = false)
        {
            if (loot.CheckSpawnAllowed())
            {
                var newLoot = Instantiate(loot, transform.position, transform.rotation);
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