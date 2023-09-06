using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Slime.Status.Effectables;

namespace SlimeBounce.Slime.Status
{
    public class ProtectedStatus : StatusEffect, IPickupEffectable
    {
        private event Func<bool> _protectionCall;

        public ProtectedStatus SetProtectionCall(Func<bool> callForProtection)
        {
            if (_protectionCall == null)
                _protectionCall = callForProtection;
            return this;
        }

        public bool VerifyPickupBlock()
        {
            bool protectionResult = _protectionCall.Invoke();
            if (!protectionResult)
                Expire();
            return protectionResult;
        }

        public override bool MergeStatus(StatusEffect otherStatus)
        {
            bool mergeResult = false;
            if (otherStatus is ProtectedStatus)
            {
                _remainingDuration = Duration;
                mergeResult = true;
            }
            return mergeResult;
        }
    }
}