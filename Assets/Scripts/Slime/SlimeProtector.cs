using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Slime.Status;
using SpecialEffects;
using Zenject;

namespace SlimeBounce.Slime
{
    public sealed class SlimeProtector : SlimeCore
    {
        [Header("Protector Parameters")]
        [Space(10)]
        [SerializeField] private Aura _protectorAura;
        [SerializeField] private FX _vfxAuraBreak;
        [SerializeField] private FX _vfxAuraDefend;
        [SerializeField] private int _protectionDurability;

        [Inject]
        private StatusEffect.Factory _statusFactory;

        protected override void Start()
        {
            base.Start();
            _protectorAura.OnAuraApplied += ApplyProtectionStatus;
        }

        private bool CheckProtection()
        {
            bool protectionWorked = _protectionDurability-- >= 0;
            if (_protectorAura.IsActive)
            {
                if (_protectionDurability <= 0)
                {
                    _protectorAura.Dissipate();
                    _visuals.AttachVfx(_vfxAuraBreak);
                }
                else
                {
                    _visuals.AttachVfx(_vfxAuraDefend);
                }
            }
            return protectionWorked;
        }

        private void ApplyProtectionStatus(SlimeCore target)
        {
            target.ApplyStatusEffect(_statusFactory.Create<ProtectedStatus>().SetProtectionCall(CheckProtection));
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void OnClickStart()
        {
            base.OnClickStart();
            if (_protectorAura.IsActive)
            {
                _protectionDurability--;
                _visuals.AttachVfx(_vfxAuraBreak);
            }
            PickByPlayer();
        }

        protected override void OnClickEnd()
        {
            base.OnClickEnd();
            DropByPlayer();
        }

        public override void Despawn()
        {
            base.Despawn();
            _protectionDurability = -1;
            _protectorAura.Dissipate();
            //Spawn some VFX if need be
        }
    }
}
