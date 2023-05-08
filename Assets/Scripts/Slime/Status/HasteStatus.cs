using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.Slime.Status
{
    public class HasteStatus : StatusEffect
    {
        public HasteStatus()
        {
            Initialize();
        }

        public override bool MergeStatus(StatusEffect otherStatus)
        {
            bool mergeResult = false;
            if (otherStatus is HasteStatus)
            {
                if (Duration < otherStatus.Duration)
                {
                    Duration = otherStatus.Duration;
                    remainingDuration = Duration;
                }
                mergeResult = true;
            }
            return mergeResult;
        }
    }
}