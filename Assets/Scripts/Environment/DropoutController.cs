using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Environment.DropoutComponents;
using SlimeBounce.Settings;
using SlimeBounce.Player;

namespace SlimeBounce.Environment
{
    public class DropoutController : MonoBehaviour
    {
        [SerializeField] DropoutSpawner[] dropoutSpawns;
        

        // Start is called before the first frame update
        void Start()
        {
            DropoutZoneData spawnerData = new DropoutZoneData();
            spawnerData.Cooldown = DropoutSettings.GetDropoutBaseCooldown() * (1f - 0.01f * UpgradeController.GetUpgradeValue(UpgradeType.FasterDropzone));
            int dropoutZoneCount = DropoutSettings.GetDropoutBaseAmount() + (int)UpgradeController.GetUpgradeValue(UpgradeType.NewDropzone);
            for (var i = 0; i < dropoutSpawns.Length; i++)
            {
                dropoutSpawns[i].SpawnZone(spawnerData);
            }

            LevelController.OnLevelEnded += OnLevelEnd;
        }

        private void OnDestroy()
        {
            LevelController.OnLevelEnded -= OnLevelEnd;
        }

        void OnLevelEnd(bool isWin)
        {
            foreach (var zone in dropoutSpawns)
            {
                zone.HideSpawnedZone();
            }
        }
    }
}