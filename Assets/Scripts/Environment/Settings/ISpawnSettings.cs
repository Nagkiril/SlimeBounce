using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Slime;

namespace SlimeBounce.Environment.Settings
{
    public interface ISpawnSettings
    {
        public float GetSpawnDelay();

        public SlimeCore GetSpawnPrefab(SlimeType slimeType);

    }
}