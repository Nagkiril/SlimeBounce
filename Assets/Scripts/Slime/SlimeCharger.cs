using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SlimeBounce.Slime.Visuals;
using SlimeBounce.Slime.Status;

namespace SlimeBounce.Slime
{
    public sealed class SlimeCharger : SlimeCore
    {
        [Header("Charger Parameters")]
        [Space(10)]
        [SerializeField] float chargeMovementSpeed;
        [SerializeField] float chargeCooldown;
        [SerializeField] float chargeDuration;
        Sequence _chargeSequence;

        bool _chargeStunned;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            ChangeChargePhase(false);
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }

        protected override void OnClickStart()
        {
            base.OnClickStart();
            PickByPlayer();
        }

        protected override void OnClickEnd()
        {
            base.OnClickEnd();
            DropByPlayer();
        }

        protected override void OnFloorTouch()
        {
            base.OnFloorTouch();
            if (!_chargeStunned)
            {
                ApplyStatusEffect(new StunStatus());
            }
        }

        protected override void OnStatusExpired(StatusEffect targetEffect)
        {
            base.OnStatusExpired(targetEffect);
            if (!_chargeStunned && targetEffect is StunStatus)
            {
                chargeMovementSpeed *= 0.5f;
                _chargeStunned = true;
            }
        }

        public override void Despawn()
        {
            base.Despawn();
            //Spawn some VFX if need be
        }



        private void ChangeChargePhase(bool isCharging)
        {
            _chargeSequence = DOTween.Sequence();
            _chargeSequence.AppendInterval((isCharging ? chargeDuration : chargeCooldown));
            _chargeSequence.AppendCallback(() => ChangeChargePhase(!isCharging));
            movement.MovementSpeed = (isCharging ? chargeMovementSpeed : baseMovementSpeed );
            visuals.SetCustomState("Charging", isCharging);
        }
    }
}
