using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Settings;
using SlimeBounce.Player.Settings;

namespace SlimeBounce.Abilities.Settings
{
    public interface IAbilitySettings
    {
        public List<UpgradeType> GetLinkedUpgrades();
        public int GetAbilityCooldown(UpgradeType abilityType);
        public AbilityCore GetAbilityObject(UpgradeType abilityType);

    }
}