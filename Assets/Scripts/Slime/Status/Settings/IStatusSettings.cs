using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpecialEffects;

namespace SlimeBounce.Slime.Status.Settings
{
    public interface IStatusSettings
    {
        public FX GetDefaultVfx(StatusEffect status);
        public float GetDefaultDuration(StatusEffect status);
    }
}