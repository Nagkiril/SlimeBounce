using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Player.Settings;

namespace SlimeBounce.Abilities
{
    public interface ICooldownHandler
    {
        event Action OnCooldownExpired;
        event Action OnCooldownStarted;

        bool IsCooldownActive { get; }


        void ResetCooldown();
        void Recharge();
        UpgradeType GetHandledType();
    }
}