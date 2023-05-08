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
        [SerializeField] FX indicatorVfx;
        [SerializeField] FX popVfx;
        [SerializeField] float popTimer;
        [SerializeField] SphereCollider popAoE;

        Sequence _popSequence;


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

        void Detonate()
        {
            if (_popSequence != null)
            {
                _popSequence.Kill();
                _popSequence = null;
                Instantiate(popVfx, transform.position, Quaternion.identity);
                Destroy(gameObject);
                foreach (var collider in Physics.OverlapSphere(popAoE.transform.position, popAoE.transform.localScale.x / 2f))
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
            if (_popSequence == null)
            {
                _popSequence = DOTween.Sequence();
                _popSequence.AppendInterval(popTimer).AppendCallback(Detonate);
                visuals.SetCustomState("Popping", true);
                movement.MovementSpeed = 0;
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
