using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpecialEffects;
using DG.Tweening;
using SlimeBounce.Slime.Control;

namespace SlimeBounce.Slime
{
    public sealed class SlimePopping : SlimeCore
    {
        [Header("Popping Parameters")]
        [Space(10)]
        [SerializeField] private FX _indicatorVfx;
        [SerializeField] private FX _popVfx;
        [SerializeField] private float _popTimer;
        [SerializeField] private SphereCollider _popAoE;

        private Sequence _popSequence;

        protected override void Start()
        {
            base.Start();
        }

        private void Detonate()
        {
            if (_popSequence != null)
            {
                _popSequence.Kill();
                _popSequence = null;
                Instantiate(_popVfx, transform.position, Quaternion.identity);
                Despawn();
                foreach (var collider in Physics.OverlapSphere(_popAoE.transform.position, _popAoE.transform.localScale.x / 2f))
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
            if (_popSequence == null)
            {
                _popSequence = DOTween.Sequence();
                _popSequence.AppendInterval(_popTimer).AppendCallback(Detonate);
                _visuals.SetCustomState("Popping", true);
                _movement.MovementSpeed = 0;
            }
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void OnClickStart()
        {
            base.OnClickStart();
            LightFuse();
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
