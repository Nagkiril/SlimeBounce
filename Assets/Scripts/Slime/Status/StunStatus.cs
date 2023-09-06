using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Slime.Status.Effectables;

namespace SlimeBounce.Slime.Status
{
    public class StunStatus : StatusEffect, IBindEffectable
    {
        public StunStatus SetStunDuration(float duration)
        {
            Duration = duration;
            return this;
        }

        public override bool MergeStatus(StatusEffect otherStatus)
        {
            bool mergeResult = false;
            if (otherStatus is StunStatus)
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

        public bool VerifyBinding()
        {
            return true;
        }
    }
}