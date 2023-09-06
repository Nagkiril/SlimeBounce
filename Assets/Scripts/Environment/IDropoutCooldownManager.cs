using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.Environment
{
    public interface IDropoutCooldownManager
    {
        public event Action OnCooldownStarted;

        public void AddCooldownMultiplier(float multiplier);

        public void ResetCooldowns();
    }
}