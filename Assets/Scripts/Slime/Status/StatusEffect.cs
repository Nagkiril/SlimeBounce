using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpecialEffects;
using SlimeBounce.Settings;
using SlimeBounce.Slime.Status.Settings;
using Zenject;

namespace SlimeBounce.Slime.Status
{
    public abstract class StatusEffect 
    {
        //Pretty sure we don't really need any of these attributes... I'm thinking that maybe we could refactor status effects so that they can be serialized and read comfortably for any kind of status effect
        [field: SerializeField] public FX StatusVfx { get; protected set; }
        [field: SerializeField] public float Duration { get; protected set; }

        protected float _remainingDuration;

        public event Action<StatusEffect> OnDurationExpired;

        [Inject]
        IStatusSettings _statusSettings;

        protected virtual void Initialize()
        {
            var defaultVfx = _statusSettings.GetDefaultVfx(this);
            var defaultDuration = _statusSettings.GetDefaultDuration(this);
            if (defaultVfx != null)
                StatusVfx = defaultVfx;
            if (defaultDuration != 0)
                Duration = defaultDuration;
        }

        public virtual void Begin()
        {
            _remainingDuration = Duration;
        }

        public virtual void Expire()
        {
            OnDurationExpired?.Invoke(this);
        }

        public virtual void ApplyTick(float tickDelta)
        {
            _remainingDuration -= tickDelta;
            if (_remainingDuration <= 0f)
                Expire();
        }

        public abstract bool MergeStatus(StatusEffect otherStatus);

        //We're not using an interface here, because we can't really create the status effect itself, but we can create their subclasses without defining lots of factories
        //In hindisght, I think having injectable StatusEffects is a bit more headache than its worth; I'd prefer to not have injection here, that way they become much less weighty and much more scalable
        //Either a semi-singleton static settings class is ok, or just generally keep 'em as clean as possible
        //Factory like this seems lightweight enough though, perhaps factory itself could be injected with settings and status effect itself would be clean?
        public class Factory
        {
            [Inject]
            readonly DiContainer _container = null;

            public T Create<T>() where T : StatusEffect, new()
            {
                T newEffect = new T();
                _container.Inject(newEffect);
                newEffect.Initialize();
                return newEffect;
            }
        }
    }
}