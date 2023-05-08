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
        [SerializeField] FX auraVisual;
        [SerializeField] AuraCollision collision;
        [SerializeField] float pulsePause;

        public event Action<SlimeCore> OnAuraApplied;
        Sequence _pulseSequence;

        public bool IsActive { get; private set;}


        // Start is called before the first frame update
        void Start()
        {
            IsActive = true;
            collision.OnSlimeEnter += ApplyAura;
            ResolveAuraPulse();
        }

        private void ResolveAuraPulse()
        {
            if (gameObject != null)
            {
                foreach (var target in collision.AffectedSlimes)
                {
                    ApplyAura(target);
                }
                _pulseSequence = DOTween.Sequence().AppendInterval(pulsePause).AppendCallback(ResolveAuraPulse);
            }
        }

        private void ApplyAura(SlimeCore target)
        {
            OnAuraApplied?.Invoke(target);
        }

        public void Dissipate()
        {
            auraVisual.Hide();
            _pulseSequence.Kill();
            collision.gameObject.SetActive(false);
            IsActive = false;
        }
    }
}