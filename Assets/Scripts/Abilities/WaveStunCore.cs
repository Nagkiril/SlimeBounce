using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Abilities.Components;
using SlimeBounce.Slime;
using SlimeBounce.Slime.Status;
using DG.Tweening;
using Zenject;

namespace SlimeBounce.Abilities
{
    public class WaveStunCore : AbilityCore
    {
        [SerializeField] private AbilityCollider _waveCollider;
        [SerializeField] private SlimeEffector _effector;
        [SerializeField] private float _waveDuration;
        [SerializeField] private AbilityCollider _destroyCollider;
        [SerializeField] private float _knockbackDuration;
        [SerializeField] private float _knockbackStrength;
        [SerializeField] private float _slowMultiplier;

        private Sequence _travelSequence;
        private float _stunDuration;
        private float _slowDuration;

        [Inject]
        private StatusEffect.Factory _statusFactory;

        override protected void Awake()
        {
            base.Awake();
            _waveCollider.OnSlimeEntry += OnWaveTouched;
        }

        protected override void Initialize(List<float> abilityStats, bool isEnhanced)
        {
            IsEnhanced = isEnhanced;
            _stunDuration = abilityStats[0];
            _slowDuration = _stunDuration + abilityStats[1];
            _travelSequence = DOTween.Sequence();
            _travelSequence.Append(_waveCollider.transform.DOLocalMove(_destroyCollider.transform.localPosition, _waveDuration).SetEase(Ease.Linear));
            if (isEnhanced) 
            {
                _travelSequence.AppendCallback(ExplodeWave);
            } 
            else
            {
                _destroyCollider.gameObject.SetActive(false);
            }
            _travelSequence.AppendCallback(EndAbility);
        }

        private void OnWaveTouched(SlimeCore slime)
        {
            if (!_effector.IsAffected(slime))
            {
                slime.ApplyStatusEffect(_statusFactory.Create<StunStatus>().SetStunDuration(_stunDuration));
                slime.ApplyStatusEffect(_statusFactory.Create<SlowStatus>().SetSlowValues(_slowMultiplier, _slowDuration));
                if (IsEnhanced)
                {
                    slime.ApplyStatusEffect(_statusFactory.Create<KnockbackStatus>().SetKnockbackValues(transform.forward * _knockbackStrength, _knockbackDuration));
                }
                _effector.ApplyEffects(slime);
            }
        }
        
        private void ExplodeWave()
        {
            foreach (var slime in _destroyCollider.GetInsideSlimes())
            {
                slime.Despawn();
            }
        }

        protected override void Finish()
        {
            _waveCollider.enabled = false;
            _destroyCollider.enabled = false;
        }
    }
}