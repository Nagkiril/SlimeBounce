using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpecialEffects;
using DG.Tweening;
using SlimeBounce.Slime.Control;

namespace SlimeBounce.Slime
{
    public sealed class SlimeFlamer : SlimeCore
    {
        [Header("Flamer Parameters")]
        [Space(10)]
        [SerializeField] FX indicatorVfx;
        [SerializeField] FX castVfx;
        [SerializeField] float castDelay;
        [SerializeField] float castDuration;
        [SerializeField] float castSlowdown;
        [SerializeField] SphereCollider spellAoE;

        Sequence _castSequence;
        Sequence _precastSequence;


        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            _precastSequence = DOTween.Sequence();
            _precastSequence.AppendInterval(castDelay).AppendCallback(StartCast);
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

        void InterruptCast()
        {
            if (_castSequence != null)
            {
                movement.MovementSpeed /= castSlowdown;
                visuals.SetCustomState("Flaming", false);

                _castSequence.Kill();
                _castSequence = null;
            }
            if (_precastSequence != null)
            {
                _precastSequence.Kill();
                _precastSequence = null;
            }
            indicatorVfx.Hide();
        }

        void FinishCast()
        {
            if (_castSequence != null)
            {
                InterruptCast();
                Instantiate(castVfx, transform.position, Quaternion.identity);
                foreach (var collider in Physics.OverlapSphere(spellAoE.transform.position, spellAoE.transform.localScale.x / 2f))
                {
                    var slimeCollider = collider.GetComponent<SlimeCollision>();
                    if (slimeCollider != null && slimeCollider.Slime != this)
                    {
                        slimeCollider.Slime.ApplyStatusEffect(new Status.HasteStatus());
                    }
                }
            }
        }

        void StartCast()
        {
            if (_castSequence == null)
            {
                _castSequence = DOTween.Sequence();
                _castSequence.AppendInterval(castDuration).AppendCallback(FinishCast);
                visuals.SetCustomState("Flaming", true);
                movement.MovementSpeed *= castSlowdown;
            }
        }

        public override void Despawn()
        {
            base.Despawn();
            //Spawn some VFX if need be
            InterruptCast();
        }
    }
}
