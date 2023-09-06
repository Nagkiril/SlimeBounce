using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SlimeBounce.Slime.Visuals;
using SlimeBounce.Slime.Status;
using Zenject;

namespace SlimeBounce.Slime
{
    public sealed class SlimeCharger : SlimeCore
    {
        [Header("Charger Parameters")]
        [Space(10)]
        [SerializeField] private float _chargeMovementSpeed;
        [SerializeField] private float _chargeCooldown;
        [SerializeField] private float _chargeDuration;
        private Sequence _chargeSequence;

        private bool _chargeStunned;
        
        [Inject]
        private StatusEffect.Factory _statusFactory;

        protected override void Start()
        {
            base.Start();
            ChangeChargePhase(false);
        }

        private void ChangeChargePhase(bool isCharging)
        {
            _chargeSequence = DOTween.Sequence();
            _chargeSequence.AppendInterval((isCharging ? _chargeDuration : _chargeCooldown));
            _chargeSequence.AppendCallback(() => ChangeChargePhase(!isCharging));
            _movement.MovementSpeed = (isCharging ? _chargeMovementSpeed : _baseMovementSpeed);
            _visuals.SetCustomState("Charging", isCharging);
        }

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
                ApplyStatusEffect(_statusFactory.Create<StunStatus>());
            }
        }

        protected override void OnStatusExpired(StatusEffect targetEffect)
        {
            base.OnStatusExpired(targetEffect);
            if (!_chargeStunned && targetEffect is StunStatus)
            {
                _chargeMovementSpeed *= 0.5f;
                _chargeStunned = true;
            }
        }

        public override void Despawn()
        {
            base.Despawn();
            //Spawn some VFX if need be
        }
    }
}
