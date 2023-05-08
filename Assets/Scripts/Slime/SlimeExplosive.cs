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
        [SerializeField] FX explosionVfx;
        [SerializeField] float explosionTimer;
        [SerializeField] SphereCollider explosionAoE;

        Sequence _explosiveSequence;


        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
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

        void Detonate()
        {
            if (_explosiveSequence != null)
            {
                _explosiveSequence.Kill();
                _explosiveSequence = null;
                Instantiate(explosionVfx, transform.position, Quaternion.identity);
                Destroy(gameObject);
                foreach (var collider in Physics.OverlapSphere(explosionAoE.transform.position, explosionAoE.transform.localScale.x / 2f))
                {
                    var slimeCollider = collider.GetComponent<SlimeCollision>();
                    if (slimeCollider != null)
                    {
                        Destroy(slimeCollider.Slime.gameObject);
                    }
                }
            }
        }

        void LightFuse()
        {
            if (_explosiveSequence == null)
            {
                _explosiveSequence = DOTween.Sequence();
                _explosiveSequence.AppendInterval(explosionTimer).AppendCallback(Detonate);
                visuals.SetCustomState("Exploding", true);
            }
        }

        public override void Despawn()
        {
            base.Despawn();
            //Spawn some VFX if need be
            Detonate();
        }
    }
}
