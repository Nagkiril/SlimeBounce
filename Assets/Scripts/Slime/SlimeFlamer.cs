using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpecialEffects;
using DG.Tweening;
using SlimeBounce.Slime.Control;
using SlimeBounce.Slime.Status;
using Zenject;

namespace SlimeBounce.Slime
{
    public sealed class SlimeFlamer : SlimeCore
    {
        [Header("Flamer Parameters")]
        [Space(10)]
        [SerializeField] private FX _indicatorVfx;
        [SerializeField] private FX _castVfx;
        [SerializeField] private float _castDelay;
        [SerializeField] private float _castDuration;
        [SerializeField] private float _castSlowdown;
        [SerializeField] private SphereCollider _spellAoE;

        private Sequence _castSequence;
        private Sequence _precastSequence;

        [Inject]
        StatusEffect.Factory _statusFactory;

        protected override void Start()
        {
            base.Start();
            _precastSequence = DOTween.Sequence();
            _precastSequence.AppendInterval(_castDelay).AppendCallback(StartCast);
        }

        private void InterruptCast()
        {
            if (_castSequence != null)
            {
                _movement.MovementSpeed /= _castSlowdown;
                _visuals.SetCustomState("Flaming", false);

                _castSequence.Kill();
                _castSequence = null;
            }
            if (_precastSequence != null)
            {
                _precastSequence.Kill();
                _precastSequence = null;
            }
            _indicatorVfx.Hide();
        }

        private void FinishCast()
        {
            if (_castSequence != null)
            {
                InterruptCast();
                Instantiate(_castVfx, transform.position, Quaternion.identity);
                foreach (var collider in Physics.OverlapSphere(_spellAoE.transform.position, _spellAoE.transform.localScale.x / 2f))
                {
                    var slimeCollider = collider.GetComponent<SlimeCollision>();
                    if (slimeCollider != null && slimeCollider.Slime != this)
                    {
                        slimeCollider.Slime.ApplyStatusEffect(_statusFactory.Create<HasteStatus>());
                    }
                }
            }
        }

        private void StartCast()
        {
            if (_castSequence == null)
            {
                _castSequence = DOTween.Sequence();
                _castSequence.AppendInterval(_castDuration).AppendCallback(FinishCast);
                _visuals.SetCustomState("Flaming", true);
                _movement.MovementSpeed *= _castSlowdown;
            }
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void OnClickStart()
        {
            base.OnClickStart();
            if (PickByPlayer())
            {
                InterruptCast();
            }
        }

        protected override void OnFloorTouch()
        {
            base.OnFloorTouch();
            FinishCast();
        }

        protected override void OnClickEnd()
        {
            base.OnClickEnd();
            DropByPlayer();
        }

        public override void Despawn()
        {
            base.Despawn();
            //Spawn some VFX if need be
            InterruptCast();
        }
    }
}
