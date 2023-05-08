using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.Slime.Status
{
    public class ProtectedStatus : StatusEffect
    {
        event Func<bool> _protectionCall;
        

        public ProtectedStatus(Func<bool> callForProtection)
        {
            Initialize();
            _protectionCall = callForProtection;
        }

        public bool VerifyProtection()
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
                remainingDuration = Duration;
                mergeResult = true;
            }
            return mergeResult;
        }
    }
}