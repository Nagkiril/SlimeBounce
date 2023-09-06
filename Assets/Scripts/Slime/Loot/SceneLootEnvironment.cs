using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using SlimeBounce.Abilities;

namespace SlimeBounce.Slime.Loot
{
    public class SceneLootEnvironment : MonoBehaviour, ILootEnvironmentProvider
    {
        [Inject]
        private IAbilityCooldownHub _cooldownHub;

        private LootEnvironment _environment;

        private void FormLootEnvironment()
        {
            if (_environment == null)
            {
                _environment = new LootEnvironment();
                _environment.CooldownHub = _cooldownHub;
            }
        }

        public LootEnvironment GetEnvironment()
        {
            FormLootEnvironment();
            return _environment;
        }

    }
}