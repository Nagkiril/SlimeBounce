using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.Environment.DropoutComponents
{
    public class DropoutSpawner : MonoBehaviour
    {
        [SerializeField] DropoutZone dropoutPrefab;
        [SerializeField] DropoutCanvasIndicator zoneIndicator;
        [SerializeField] GameObject visualIndicator;
        

        private DropoutZone _spawnedZone;
        public bool IsFree => _spawnedZone == null;


        private void Awake()
        {
            visualIndicator.SetActive(false);
        }

        public bool SpawnZone(DropoutZoneData spawnerData)
        {
            if (IsFree)
            {
                spawnerData.SpawnedIndicator = zoneIndicator;
                _spawnedZone = Instantiate(dropoutPrefab, transform);
                _spawnedZone.Initialize(spawnerData);
                return true;
            } else
            {
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
    }
}