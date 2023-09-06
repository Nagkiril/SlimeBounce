using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpecialEffects;
using SlimeBounce.Slime.Status.Effectables;

namespace SlimeBounce.Slime.Status
{
    public class SlimeStatusController : MonoBehaviour
    {
        public bool IsBound { get; private set; }
        public float SpeedMultiplier { get; private set; }
        public bool PickUpVerified => VerifyPickUp();

        public event Action<StatusEffect> OnStatusExpired;

        private List<StatusEffect> _activeEffects;
        private List<IMoveEffectable> _statusMovement;

        private void Start()
        {
            _activeEffects = new List<StatusEffect>();
            _statusMovement = new List<IMoveEffectable>();
            SpeedMultiplier = 1;
        }

        private void Update()
        {
            for (var i = _activeEffects.Count - 1; i >= 0; i--)
            {
                _activeEffects[i].ApplyTick(Time.deltaTime);
            }
        }

        private void StatusExpired(StatusEffect target)
        {
            _activeEffects.Remove(target);

            if (target is IBindEffectable)
            {
                VerifyBinding();
            }
            if (target is ISpeedEffectable speedEffect)
            {
                SpeedMultiplier /= speedEffect.GetSpeedMultiplier();
            }
            if (target is IMoveEffectable move)
            {
                _statusMovement.Remove(move);
            }

            OnStatusExpired?.Invoke(target);
        }

        private void VerifyBinding()
        {
            IsBound = false;
            foreach (var effect in _activeEffects)
            {
                if (effect is IBindEffectable bindEffectable)
                {
                    IsBound = bindEffectable.VerifyBinding();
                    if (IsBound)
                        break;
                }
            }
        }

        private bool VerifyPickUp()
        {
            bool canBePickedUp = true;
            for (var i = _activeEffects.Count - 1; i >= 0; i--)
            {
                if (_activeEffects[i] is IPickupEffectable protection)
                {
                    canBePickedUp = !protection.VerifyPickupBlock();
                    if (!canBePickedUp)
                        break;
                }
            }
            return canBePickedUp;
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

                if (effect is ISpeedEffectable speedEffect)
                {
                    SpeedMultiplier *= speedEffect.GetSpeedMultiplier();
                }

                if (effect is IBindEffectable)
                {
                    VerifyBinding();
                }

                if (effect is IMoveEffectable move)
                {
                    _statusMovement.Add(move);
                }
            }
            return !isMerged;
        }

        public Vector3 GetStatusMove()
        {
            Vector3 moveResult = new Vector3();
            foreach (var move in _statusMovement)
            {
                moveResult += move.GetMoveVector();
            }
            return moveResult;
        }

        public void Clear()
        {
            for (var i = _activeEffects.Count - 1; i >= 0 ; i--)
            {
                _activeEffects[i].Expire();
            }
        }
    }
}