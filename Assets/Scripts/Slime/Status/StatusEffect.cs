using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpecialEffects;
using SlimeBounce.Settings;

namespace SlimeBounce.Slime.Status
{
    public abstract class StatusEffect 
    {
        [field: SerializeField] public FX StatusVfx { get; protected set; }
        [field: SerializeField] public float Duration { get; protected set; }

        protected float remainingDuration;

        public event Action<StatusEffect> OnDurationExpired;

        protected virtual void Initialize()
        {
            StatusVfx = StatusEffectSettings.GetDefaultVfx(this);
            Duration = StatusEffectSettings.GetDefaultDuration(this);
        }

        public virtual void Begin()
        {
            remainingDuration = Duration;
        }

        public virtual void Expire()
        {
            OnDurationExpired?.Invoke(this);
        }

        public virtual void ApplyTick(float tickDelta)
        {
            remainingDuration -= tickDelta;
            if (remainingDuration <= 0f)
                Expire();
        }

        public abstract bool MergeStatus(StatusEffect otherStatus);
    }
}