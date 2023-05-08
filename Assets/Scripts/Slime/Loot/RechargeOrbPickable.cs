using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Environment;

namespace SlimeBounce.Slime.Loot
{
    public class RechargeOrbPickable : PickableLoot
    {

        protected override void ApplyLootEffects()
        {
            LevelController.RechargeAbilities();
        }

        public override bool CheckSpawnAllowed()
        {
            return LevelController.HasCooldownAbilities;
        }
    }
}