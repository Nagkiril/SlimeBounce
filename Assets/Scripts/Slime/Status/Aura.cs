using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Slime.Status.AuraComponents;
using SpecialEffects;
using DG.Tweening;

namespace SlimeBounce.Slime.Status
{
    public class Aura : MonoBehaviour
    {
        [SerializeField] private FX _auraVisual;
        [SerializeField] private AuraCollision _collision;
        [SerializeField] private float _pulsePause;
        private Sequence _pulseSequence;

        public event Action<SlimeCore> OnAuraApplied;
        public bool IsActive { get; private set;}

        private void Start()
        {
            IsActive = true;
            _collision.OnSlimeEntry += ApplyAura;
            ResolveAuraPulse();
        }

        private void ResolveAuraPulse()
        {
            if (gameObject != null)
            {
                foreach (var target in _collision.AffectedSlimes)
                {
                    ApplyAura(target);
                }
                _pulseSequence = DOTween.Sequence().AppendInterval(_pulsePause).AppendCallback(ResolveAuraPulse);
            }
        }

        private void ApplyAura(SlimeCore target)
        {
            OnAuraApplied?.Invoke(target);
        }

        public void Dissipate()
        {
            _auraVisual.Hide();
            _pulseSequence.Kill();
            _collision.gameObject.SetActive(false);
            IsActive = false;
        }
    }
}