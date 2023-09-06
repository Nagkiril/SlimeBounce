using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Environment;
using SlimeBounce.Abilities;
using SlimeBounce.Settings;
using SlimeBounce.Player.Settings;
using SlimeBounce.UI.Settings;
using Zenject;

namespace SlimeBounce.UI.Abilities
{
    public class AbilityPanel : MonoBehaviour
    {
        [SerializeField] private AbilityControl _controlPrefab;

        private List<AbilityControl> _spawnedControls;

        [Inject]
        private AbilityControl.Factory _controlFactory;
        [Inject]
        private IUpgradeViewSettings _upgradeViewSettings;
        [Inject]
        private IAbilityActor _abilityActor;
        [Inject]
        private IAbilityCooldownHub _cooldownHub;
        [Inject]
        private ILevelStateProvider _levelState;

        private void Awake()
        {
            _spawnedControls = new List<AbilityControl>();
        }

        private void Start()
        {
            _levelState.OnLevelStarted += OnLevelStart;
            _levelState.OnLevelEnded += OnLevelEnd;
        }

        private void OnDestroy()
        {
            _levelState.OnLevelStarted -= OnLevelStart;
            _levelState.OnLevelEnded -= OnLevelEnd;
        }

        private void OnLevelStart()
        {
            CreateAbilityControls();
        }

        private void OnLevelEnd(bool isWin)
        {
            HideAbilityControls();
        }

        private void CreateAbilityControls()
        {
            var learntAbilities = _abilityActor.GetLearntAbilities();
            for (int i = 0; i < learntAbilities.Count; i++)
            {
                CreateAbilityControl(learntAbilities[i]);
            }
        }

        private void HideAbilityControls()
        {
            foreach (var control in _spawnedControls)
            {
                control.Hide();
            }
        }

        private void CreateAbilityControl(UpgradeType abilityType)
        {
            var newControl = _controlFactory.Create(transform);
            newControl.Initialize(abilityType, _upgradeViewSettings.GetViewForType(abilityType).Icon, _cooldownHub.GetAbilityCooldown(abilityType));
            newControl.OnControlHidden += OnControlHide;
            newControl.OnControlActivated += OnControlActivate;
            _cooldownHub.Register(newControl);
            _spawnedControls.Add(newControl);
        }

        private void OnControlActivate(AbilityControl activatedControl)
        {
            _abilityActor.UseAbility(activatedControl.GetHandledType());
        }

        private void OnControlHide(AbilityControl hiddenControl)
        {
            _cooldownHub.Unregister(hiddenControl);
            _spawnedControls.Remove(hiddenControl);
            Destroy(hiddenControl.gameObject);
        }
    }
}
