using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Player.Settings;

namespace SlimeBounce.Abilities
{
    public interface IAbilityCooldownHub
    {
        public bool HasCooldownAbilities { get; }

        public int GetAbilityCooldown(UpgradeType abilityType);

        public bool Register(ICooldownHandler handler);

        public void Unregister(ICooldownHandler handler);

        public void RechargeAbilities();

    }
}