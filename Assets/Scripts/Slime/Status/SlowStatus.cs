using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Slime.Status.Effectables;

namespace SlimeBounce.Slime.Status
{
    public class SlowStatus : StatusEffect, ISpeedEffectable
    {
        private float _speedMultiplier;

        public SlowStatus SetSlowValues(float speedMultiplier, float slowDuration)
        {
            if (speedMultiplier >= 1f)
                Debug.LogWarning("Creating a Slow Status effect that does not actually slow its recepient down!");
            _speedMultiplier = speedMultiplier;
            Duration = slowDuration;
            return this;
        }

        public float GetSpeedMultiplier() => _speedMultiplier;

        public override bool MergeStatus(StatusEffect otherStatus)
        {
            bool mergeResult = false;
            if (otherStatus is SlowStatus otherSlow)
            {
                if (Duration < otherStatus.Duration)
                {
                    Duration = otherStatus.Duration;
                    _remainingDuration = Duration;
                }
                _speedMultiplier = Mathf.Min(_speedMultiplier, otherSlow.GetSpeedMultiplier());
                mergeResult = true;
            }
            //We can also merge Haste here, if need be; I'm not doing it yet, because I'd prefer both Haste and Slow shown visually, if they both exist
            return mergeResult;
        }
    }
}