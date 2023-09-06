using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Settings.Generic;
using SlimeBounce.Slime;
using SpecialEffects;
using SlimeBounce.Slime.Status;
using SlimeBounce.Slime.Status.Settings;

namespace SlimeBounce.Settings
{
    [CreateAssetMenu(fileName = "StatusEffectSettings", menuName = "SlimeBounce/New Status Effect Settings", order = 10)]
    public class StatusEffectSettings : ScriptableObject, IStatusSettings
    {
        //It wouldn't hurt to standardize those names, if not rework the system so that new effects can be implemented easier
        [SerializeField] private FX _stunVfx;
        [SerializeField] private float _stunDuration;
        [SerializeField] private FX _protectedVFX;
        [SerializeField] private float _protectedDuration;
        [SerializeField] private FX _hasteFX;
        [SerializeField] private float _hasteDuration;

        public float GetDefaultDuration(StatusEffect status)
        {
            if (status is StunStatus)
                return _stunDuration;
            if (status is ProtectedStatus)
                return _protectedDuration;
            if (status is HasteStatus)
                return _hasteDuration;
            return 0;
        }

        public FX GetDefaultVfx(StatusEffect status)
        {
            if (status is StunStatus)
                return _stunVfx;
            if (status is ProtectedStatus)
                return _protectedVFX;
            if (status is HasteStatus)
                return _hasteFX;
            return null;
        }
    }
}