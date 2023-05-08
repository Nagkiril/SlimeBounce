using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpecialEffects;


namespace SlimeBounce.Slime.Status
{
    public class SlimeStatusController : MonoBehaviour
    {
        public bool IsBound { get; private set; }
        public float SpeedMultiplier { get; private set; }
        public bool PickUpVerified => VerifyPickUp();

        public event Action<StatusEffect> OnStatusExpired;

        List<StatusEffect> _activeEffects;

        // Start is called before the first frame update
        void Start()
        {
            _activeEffects = new List<StatusEffect>();
            SpeedMultiplier = 1;
        }

        // Update is called once per frame
        void Update()
        {
            for (var i = _activeEffects.Count - 1; i >= 0; i--)
            {
                _activeEffects[i].ApplyTick(Time.deltaTime);
            }
        }

        void StatusExpired(StatusEffect target)
        {
            if (target is StunStatus)
            {
                IsBound = false;
            }
            if (target is HasteStatus)
            {
                SpeedMultiplier /= 2;
            }

            _activeEffects.Remove(target);
            OnStatusExpired?.Invoke(target);
        }

        public bool ApplyStatusEffect(StatusEffect effect)
        {
            bool isMerged = false;
            foreach (var existingEffect in _activeEffects)
            {
                if (!isMerged)
                {
                    isMerged = existingEffect.MergeStatus(effect);
                }
            }
            if (!isMerged)
            {
                effect.OnDurationExpired += StatusExpired;
                _activeEffects.Add(effect);
                effect.Begin();

                if (effect is HasteStatus)
                {
                    SpeedMultiplier *= 2;
                }
            }

            if (effect is StunStatus)
            {
                IsBound = true;
            }
            return !isMerged;
        }

        private bool VerifyPickUp()
        {
            foreach (var existingEffect in _activeEffects)
            {
                if (existingEffect is ProtectedStatus protection)
                {
                    return !protection.VerifyProtection();
                }
            }
            return true;
        }
    }
}