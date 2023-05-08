using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Slime.Status;
using SpecialEffects;

namespace SlimeBounce.Slime
{
    public sealed class SlimeProtector : SlimeCore
    {
        [Header("Protector Parameters")]
        [Space(10)]
        [SerializeField] Aura protectorAura;
        [SerializeField] FX vfxAuraBreak;
        [SerializeField] FX vfxAuraDefend;
        [SerializeField] int protectionDurability;


        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            protectorAura.OnAuraApplied += ApplyProtectionStatus;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }

        protected override void OnClickStart()
        {
            base.OnClickStart();
            if (protectorAura.IsActive)
            {
                protectionDurability--;
                visuals.AttachVfx(vfxAuraBreak);
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
            protectionDurability = -1;
            protectorAura.Dissipate();
            //Spawn some VFX if need be
        }

        private bool CheckProtection()
        {
            bool protectionWorked = protectionDurability-- >= 0;
            if (protectorAura.IsActive)
            {
                if (protectionDurability <= 0)
                {
                    protectorAura.Dissipate();
                    visuals.AttachVfx(vfxAuraBreak);
                } else
                {
                    visuals.AttachVfx(vfxAuraDefend);
                }
            }
            return protectionWorked;
        }

        private void ApplyProtectionStatus(SlimeCore target)
        {
            target.ApplyStatusEffect(new ProtectedStatus(CheckProtection));
        }
    }
}
