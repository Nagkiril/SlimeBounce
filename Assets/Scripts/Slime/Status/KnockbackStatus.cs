using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Slime.Status.Effectables;

namespace SlimeBounce.Slime.Status
{
    public class KnockbackStatus : StatusEffect, IMoveEffectable
    {
        private Vector3 _moveVector;

        public KnockbackStatus SetKnockbackValues(Vector3 moveVector, float duration)
        {
            Duration = duration;
            _moveVector = moveVector;
            return this;
        }

        public Vector3 GetMoveVector() => _moveVector;

        public override bool MergeStatus(StatusEffect otherStatus)
        {
            bool mergeResult = false;
            if (otherStatus is KnockbackStatus otherKnockback)
            {
                if (Duration < otherStatus.Duration && otherKnockback._moveVector == _moveVector)
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