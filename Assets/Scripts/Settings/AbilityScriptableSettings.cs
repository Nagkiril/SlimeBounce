using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Player.Settings;
using SlimeBounce.Abilities;
using SlimeBounce.Abilities.Settings;

namespace SlimeBounce.Settings
{
    [CreateAssetMenu(fileName = "AbilitySettings", menuName = "SlimeBounce/New Ability Settings", order = 10)]
    public class AbilityScriptableSettings : ScriptableObject, IAbilitySettings
    {
        [SerializeField] private AbilityEntry[] _abilities;

        [Serializable]
        private class AbilityEntry
        {
            public UpgradeType LinkedUpgrade;
            public int BaseCooldown;
            public AbilityCore UsagePrefab;
        }

        private AbilityEntry GetAbilityByUpgradeType(UpgradeType linkedType)
        {
            return _abilities.First(x => x.LinkedUpgrade == linkedType);
        }

        public List<UpgradeType> GetLinkedUpgrades()
        {
            List<UpgradeType> upgrades = new List<UpgradeType>();
            foreach (var ability in _abilities)
            {
                upgrades.Add(ability.LinkedUpgrade);
            }
            return upgrades;
        }

        public int GetAbilityCooldown(UpgradeType abilityType)
        {
            var ability = GetAbilityByUpgradeType(abilityType);
            return ability != null ? ability.BaseCooldown : -1;
        }

        public AbilityCore GetAbilityObject(UpgradeType abilityType)
        {
            var ability = GetAbilityByUpgradeType(abilityType);
            return ability != null ? ability.UsagePrefab : null;
        }
    }
}