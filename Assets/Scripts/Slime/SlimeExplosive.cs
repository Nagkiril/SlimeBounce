using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpecialEffects;
using DG.Tweening;
using SlimeBounce.Slime.Control;

namespace SlimeBounce.Slime
{
    public sealed class SlimeExplosive : SlimeCore
    {
        [Header("Explosive Parameters")]
        [Space(10)]
        [SerializeField] private FX _explosionVfx;
        [SerializeField] private float _explosionTimer;
        [SerializeField] private SphereCollider _explosionAoE;

        private Sequence _explosiveSequence;

        protected override void Start()
        {
            base.Start();
        }

        private void Detonate()
        {
            if (_explosiveSequence != null)
            {
                _explosiveSequence.Kill();
                _explosiveSequence = null;
                Instantiate(_explosionVfx, transform.position, Quaternion.identity);
                Despawn();
                foreach (var collider in Physics.OverlapSphere(_explosionAoE.transform.position, _explosionAoE.transform.localScale.x / 2f))
                {
                    var slimeCollider = collider.GetComponent<SlimeCollision>();
                    if (slimeCollider != null)
                    {
                        slimeCollider.Slime.Despawn();
                    }
                }
            }
        }

        private void LightFuse()
        {
            if (_explosiveSequence == null)
            {
                _explosiveSequence = DOTween.Sequence();
                _explosiveSequence.AppendInterval(_explosionTimer).AppendCallback(Detonate);
                _visuals.SetCustomState("Exploding", true);
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
                LightFuse();
            }
        }

        protected override void OnFloorTouch()
        {
            base.OnFloorTouch();
            Detonate();
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
            Detonate();
        }
    }
}
