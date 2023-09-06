using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Abilities.Components;
using SlimeBounce.Slime;
using SlimeBounce.Slime.Status;
using Zenject;

namespace SlimeBounce.Abilities
{
    public class RoadShieldCore : AbilityCore
    {
        [SerializeField] private AbilityCollider _repelCollider;
        [SerializeField] private AbilityCollider _destroyCollider;
        [SerializeField] private float _knockbackDuration;
        [SerializeField] private float _knockbackStrength;

        private int _remainingDurability;
        private float _remainingTime;

        [Inject]
        private StatusEffect.Factory _statusFactory;

        protected override void Awake()
        {
            base.Awake();
            _repelCollider.OnSlimeEntry += OnShieldCollided;
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
            _remainingDurability = (int)abilityStats[1];
            if (!IsEnhanced)
                _destroyCollider.gameObject.SetActive(false);
        }

        private void OnShieldCollided(SlimeCore slime)
        {
            _remainingDurability--;
            slime.ApplyStatusEffect(_statusFactory.Create<KnockbackStatus>().SetKnockbackValues(transform.forward * _knockbackStrength, _knockbackDuration));
            CheckExpiration();
        }

        private void CheckExpiration()
        {
            if (_remainingDurability <= 0f || _remainingTime <= 0f)
            {
                EndAbility();
            }
        }

        protected override void Finish()
        {
            if (IsEnhanced)
            {
                foreach (var destroyTarget in _repelCollider.GetInsideSlimes())
                {
                    destroyTarget.Despawn();
                }
            }
            _repelCollider.gameObject.SetActive(false);
            _repelCollider.gameObject.SetActive(false);
        }
    }
}