using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Abilities.Components;
using SlimeBounce.Slime;
using SlimeBounce.Environment;
using Zenject;

namespace SlimeBounce.Abilities
{
    //We can still subclass from this, allowing us to have abilities that, for example, spawn multiple slimes overtime, or otherwise have unusual deployment behaviour  
    public class DeployableSpawnCore : AbilityCore
    {
        [SerializeField] private DeployableSlime _slimePrefab;

        [Inject]
        protected IDeploymentActor _deployment;

        override protected void Awake()
        {
            base.Awake();
        }

        protected override void Initialize(List<float> abilityStats, bool isEnhanced)
        {
            IsEnhanced = isEnhanced;
            var newSlime = _deployment.DeploySlime(_slimePrefab);
            ApplyAbilityParameters(newSlime, abilityStats, isEnhanced);
            EndAbility();
        }

        protected void ApplyAbilityParameters(SlimeCore slime, List<float> abilityStats, bool isEnhanced)
        {
            //This way we allow to spawn slimes that aren't necessarily going to accept ability parameters, but still guaranteed to behave properly when added to deployment slot
            if (slime is IAbilitySlime abilitySlime)
            {
                abilitySlime.SetEnhanced(isEnhanced);
                abilitySlime.ApplyParameters(abilityStats);
            }
        }

        protected override void Finish()
        {
            EndAbility();
        }
    }
}