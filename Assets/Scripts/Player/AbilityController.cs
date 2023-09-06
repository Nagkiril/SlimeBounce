using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Player.Settings;
using SlimeBounce.Player;
using Zenject;
using SlimeBounce.Abilities.Settings;

namespace SlimeBounce.Abilities
{
    public class AbilityController : MonoBehaviour, IAbilityActor, IAbilityCooldownHub
    {
        private const int UPGRADE_CD_TRESHOLD = 3;
        private List<UpgradeType> _abilityUpgradeTypes;
        private List<ICooldownHandler> _cooldownHandlers;
        private List<AbilityCore> _activeAbilities;

        [Inject]
        private IAbilitySettings _abilitySettings;
        [Inject]
        private AbilityCore.Factory _abilityFactory;
        [Inject]
        private IUpgradeDataProvider _upgradeData;

        public bool HasCooldownAbilities { get; private set; }

        private void Awake()
        {
            _abilityUpgradeTypes = _abilitySettings.GetLinkedUpgrades();
            _activeAbilities = new List<AbilityCore>();
            HasCooldownAbilities = false;
        }

        private void SpawnAbility(AbilityCore abilityPrefab, List<float> abilityStats, bool isEnhanced)
        {
            var usedAbility = _abilityFactory.Create(abilityPrefab);
            usedAbility.transform.SetParent(transform, false);
            _activeAbilities.Add(usedAbility);
            usedAbility.OnAbilityEnded += OnAbilityEnd;
            usedAbility.StartAbility(abilityStats, isEnhanced);
        }

        private void OnAbilityEnd(AbilityCore ability)
        {
            Destroy(ability.gameObject, 5f);
            _activeAbilities.Remove(ability);
        }

        private void OnCooldownExpire()
        {
            HasCooldownAbilities = false;
            foreach (var handler in _cooldownHandlers)
            {
                HasCooldownAbilities = handler.IsCooldownActive;
                if (HasCooldownAbilities)
                    break;
            }
        }

        private void OnCooldownStart()
        {
            HasCooldownAbilities = true;
        }

        public List<UpgradeType> GetLearntAbilities()
        {
            var availableAbilities = new List<UpgradeType>();
            foreach (var ability in _abilityUpgradeTypes)
            {
                if (PlayerData.GetUpgradeLevel(ability) > 0)
                {
                    availableAbilities.Add(ability);
                }
            }
            return availableAbilities;
        }

        public void UseAbility(UpgradeType abilityType)
        {
            var abilityStats = _upgradeData.GetUpgradeValues(abilityType);
            var isAbilityEnhanced = _upgradeData.GetCurrentUpgradeLevel(abilityType) == _upgradeData.GetMaxUpgradeLevel(abilityType);
            SpawnAbility(_abilitySettings.GetAbilityObject(abilityType), abilityStats, isAbilityEnhanced);
        }

        public int GetAbilityCooldown(UpgradeType abilityType)
        {
            var cooldown = _abilitySettings.GetAbilityCooldown(abilityType);
            var upgradeLevel = PlayerData.GetUpgradeLevel(abilityType);
            if (upgradeLevel >= UPGRADE_CD_TRESHOLD)
                cooldown--;
            return cooldown;
        }

        public bool Register(ICooldownHandler handler)
        {
            if (_cooldownHandlers == null)
                _cooldownHandlers = new List<ICooldownHandler>();
            foreach (var existingHandler in _cooldownHandlers)
            {
                if (existingHandler.GetHandledType() == handler.GetHandledType())
                {
                    return false;
                }
            }
            _cooldownHandlers.Add(handler);
            handler.OnCooldownExpired += OnCooldownExpire;
            handler.OnCooldownStarted += OnCooldownStart;
            return true;
        }

        public void Unregister(ICooldownHandler handler)
        {
            _cooldownHandlers.Remove(handler);
            handler.OnCooldownExpired -= OnCooldownExpire;
            handler.OnCooldownStarted -= OnCooldownStart;
        }

        public void RechargeAbilities()
        {
            foreach (var handler in _cooldownHandlers)
            {
                handler.Recharge();
            }
        }
    }
}