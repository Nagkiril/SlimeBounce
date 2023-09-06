using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Player.Settings;

namespace SlimeBounce.Abilities
{
    public interface IAbilityActor
    {
        public List<UpgradeType> GetLearntAbilities();

        public void UseAbility(UpgradeType abilityType);

    }
}