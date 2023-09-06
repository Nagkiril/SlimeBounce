using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Abilities.Components;
using SlimeBounce.Environment;
using Zenject;

namespace SlimeBounce.Slime.Status.AuraComponents
{
    public class VoidAuraHandler : MonoBehaviour
    {
        [SerializeField] private Aura _voidAura;
        [SerializeField] private float _gravityPullDuration;
        [SerializeField] private float _pullStrengthBase;
        [SerializeField] private float _slowMultiplier;
        [SerializeField] private AbilityCollider _despawnCollider;

        private float _remainingTime;
        private float _pullStrengthMultiplier;
        private bool _slowEnabled;

        [Inject]
        private StatusEffect.Factory _statusFactory;
        [Inject]
        private ILevelStateProvider _levelState;

        private void Awake()
        {
            _voidAura.OnAuraApplied += ApplyVortexKnockback;
            _despawnCollider.OnSlimeEntry += OnDespawnEnter;
            _levelState.OnLevelEnded += OnLevelEnd;
        }

        private void OnDestroy()
        {
            _levelState.OnLevelEnded -= OnLevelEnd;
            Expire();
        }

        private void FixedUpdate()
        {
            if (_remainingTime > 0)
            {
                _remainingTime -= Time.deltaTime;
                if (_remainingTime <= 0f)
                {
                    Expire();
                }
            }
        }

        private void OnLevelEnd(bool isWin)
        {
            Expire();
        }

        private void Expire()
        {
            _remainingTime = 0f;
            _voidAura.OnAuraApplied -= ApplyVortexKnockback;
            _despawnCollider.OnSlimeEntry -= OnDespawnEnter;
            _voidAura.Dissipate();
            Destroy(gameObject, 5f);
        }

        private void OnDespawnEnter(SlimeCore target)
        {
            target.Despawn();
        }

        private void ApplyVortexKnockback(SlimeCore slime)
        {
            Vector3 pullDirection = (transform.position - slime.transform.position).normalized;
            pullDirection.y = 0;
            slime.ApplyStatusEffect(_statusFactory.Create<KnockbackStatus>().SetKnockbackValues(pullDirection * _pullStrengthBase * _pullStrengthMultiplier, _gravityPullDuration));
            if (_slowEnabled)
            {
                slime.ApplyStatusEffect(_statusFactory.Create<SlowStatus>().SetSlowValues(_slowMultiplier, _gravityPullDuration));
            }
        }

        public void Initialize(bool enableSlowdown, float pullStrength, float vortexDuration)
        {
            _slowEnabled = enableSlowdown;
            _pullStrengthMultiplier = pullStrength;
            _remainingTime = vortexDuration;
        }

        public class Factory : PlaceholderFactory<VoidAuraHandler>
        {

        }
    }
}