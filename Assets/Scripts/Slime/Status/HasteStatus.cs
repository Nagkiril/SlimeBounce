using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Slime.Status.Effectables;

namespace SlimeBounce.Slime.Status
{
    public class HasteStatus : StatusEffect, ISpeedEffectable
    {
        public float GetSpeedMultiplier() => 2f;

        public override bool MergeStatus(StatusEffect otherStatus)
        {
            bool mergeResult = false;
            if (otherStatus is HasteStatus)
            {
                if (Duration < otherStatus.Duration)
                {
                    Duration = otherStatus.Duration;
                    _remainingDuration = Duration;
                }
                mergeResult = true;
            }
            return mergeResult;
        }
    }
}