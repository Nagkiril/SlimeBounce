using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.Environment.Settings
{
    public interface IDropoutSettings
    {
        public float DropoutBaseCooldown { get; }

        public int DropoutBaseAmount { get; }

    }
}