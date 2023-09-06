using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Environment;
using SlimeBounce.Abilities;
using Zenject;

namespace SlimeBounce.Slime.Loot
{
    public class RechargeOrbPickable : PickableLoot
    {
        [Inject]
        IAbilityCooldownHub _cooldownHub;

        protected override void ApplyLootEffects()
        {
            _cooldownHub.RechargeAbilities();
        }

        public override bool CheckSpawnAllowed(LootEnvironment environment)
        {
            return environment.CooldownHub.HasCooldownAbilities;
        }
    }
}