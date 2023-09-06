using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Abilities.Components;
using SlimeBounce.Environment;
using SlimeBounce.Slime;
using SlimeBounce.Slime.Status;
using DG.Tweening;
using Zenject;

namespace SlimeBounce.Abilities
{
    public class FeverFeastCore : AbilityCore
    {
        [SerializeField] private AbilityCollider _fieldCollider;
        [SerializeField] private SlimeEffector _fieldEffector;
        [SerializeField] private SlimeEffector _consumeEffector;
        private float _remainingTime;
        private float _recoveryMultiplier;

        [Inject]
        private IDropoutCooldownManager _dropoutCooldown;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Update()
        {
            if (_remainingTime > 0f)
            {
                _remainingTime -= Time.deltaTime;
                CheckExpiration();
            }
        }

        protected override void Initialize(List<float> abilityStats, bool isEnhanced)
        {
            IsEnhanced = isEnhanced;
            _remainingTime = abilityStats[0];
            _recoveryMultiplier = (1 - abilityStats[1] / 100);
            _dropoutCooldown.ResetCooldowns();
            _dropoutCooldown.AddCooldownMultiplier(_recoveryMultiplier);
            _dropoutCooldown.OnCooldownStarted += OnSlimeConsumed;
        }

        private void OnSlimeConsumed()
        {
            if (IsEnhanced)
            {
                var targetSlime = GetTargetableSlime();
                if (targetSlime != null)
                {
                    _fieldEffector.ApplyEffects(targetSlime);
                    targetSlime.Despawn();
                }
            }
            _consumeEffector.ApplyEffects(null);
        }

        private SlimeCore GetTargetableSlime()
        {
            SlimeCore targetableSlime = null;
            var slimesInRange = _fieldCollider.GetInsideSlimes();

            if (slimesInRange.Count > 0)
            {
                targetableSlime = slimesInRange[UnityEngine.Random.Range(0, slimesInRange.Count)];
            }
            return targetableSlime;
        }

        private void CheckExpiration()
        {
            if (_remainingTime <= 0)
            {
                EndAbility();
            }
        }

        protected override void Finish()
        {
            if (_recoveryMultiplier != 0)
            {
                _dropoutCooldown.OnCooldownStarted -= OnSlimeConsumed;
                _dropoutCooldown.AddCooldownMultiplier(1 / _recoveryMultiplier);
                base.EndAbility();
                _recoveryMultiplier = 0f;
            }
        }
    }
}