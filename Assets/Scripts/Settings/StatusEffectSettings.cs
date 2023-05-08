using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Settings.Generic;
using SlimeBounce.Slime;
using SpecialEffects;
using SlimeBounce.Slime.Status;

namespace SlimeBounce.Settings
{
    [CreateAssetMenu(fileName = "StatusEffectSettings", menuName = "SlimeBounce/New Status Effect Settings", order = 10)]
    public class StatusEffectSettings : GenericSettings<StatusEffectSettings>
    {
        //It wouldn't hurt to standardize those names, if not rework the system so that new effects can be implemented easier
        [SerializeField] protected FX stunVfx;
        [SerializeField] protected float stunDuration;
        [SerializeField] protected FX protectedVFX;
        [SerializeField] protected float protectedDuration;
        [SerializeField] protected FX hasteFX;
        [SerializeField] protected float hasteDuration;

        private const string _loadPath = "Settings/StatusEffectSettings";
        private static StatusEffectSettings instance => (StatusEffectSettings)GetInstance(_loadPath);

        public static float GetDefaultDuration(StatusEffect status)
        {
            if (status is StunStatus)
                return instance.stunDuration;
            if (status is ProtectedStatus)
                return instance.protectedDuration;
            if (status is HasteStatus)
                return instance.hasteDuration;
            Debug.LogWarning("A special effect requested unspecified settings! Make sure to update the settings.");
            return 0;
        }

        public static FX GetDefaultVfx(StatusEffect status)
        {
            if (status is StunStatus)
                return instance.stunVfx;
            if (status is ProtectedStatus)
                return instance.protectedVFX;
            if (status is HasteStatus)
                return instance.hasteFX;
            Debug.LogWarning("A special effect requested unspecified settings! Make sure to update the settings.");
            return null;
        }
    }
}