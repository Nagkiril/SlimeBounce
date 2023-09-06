using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SlimeBounce.Abilities;
using System;
using SlimeBounce.Settings;
using SlimeBounce.UI.Abilities.CooldownComponents;
using SlimeBounce.UI.Settings;
using SlimeBounce.Player.Settings;
using Zenject;

namespace SlimeBounce.UI.Abilities
{
    public class AbilityControl : MonoBehaviour, ICooldownHandler
    {
        [SerializeField] private Animator _ownAnim;
        [SerializeField] private AEContainer _animEvents;
        [SerializeField] private Button _ownButton;
        [SerializeField] private Transform _cooldownContainer;
        [SerializeField] private Image _iconImage;
        private UpgradeType _abilityType;
        private SegmentedCooldownDial _cooldownDial;

        [Inject]
        private ICooldownViewSettings _cooldownSettings;

        public bool IsCooldownActive { get; private set; }

        public event Action OnCooldownExpired;
        public event Action OnCooldownStarted;
        public event Action<AbilityControl> OnControlHidden;
        public event Action<AbilityControl> OnControlActivated;

        private void OnCooldownPass()
        {
            _ownAnim.SetBool("Inactive", false);
            IsCooldownActive = false;
            OnCooldownExpired?.Invoke();
        }

        private void OnButtonClick()
        {
            if (!_cooldownDial.IsInCooldown)
            {
                _cooldownDial.RestartCooldown();
                _ownAnim.SetTrigger("Activate");
                _ownAnim.SetBool("Inactive", true);
                IsCooldownActive = true;
                OnCooldownStarted?.Invoke();
                OnControlActivated?.Invoke(this);
            }
        }

        private void OnHideCompletion()
        {
            OnControlHidden?.Invoke(this);
        }

        public UpgradeType GetHandledType()
        {
            return _abilityType;
        }

        public void Recharge()
        {
            _cooldownDial.RechargeSegment();
        }

        public void ResetCooldown()
        {
            while (_cooldownDial.IsInCooldown)
            {
                _cooldownDial.RechargeSegment();
            }
        }

        public void Initialize(UpgradeType abilityType, Sprite icon, int cooldown)
        {
            _abilityType = abilityType;
            _cooldownDial = Instantiate(_cooldownSettings.GetDialPrefab(cooldown), _cooldownContainer);
            _cooldownDial.OnCooldownPassed += OnCooldownPass;
            _iconImage.sprite = icon;
            _ownButton.onClick.AddListener(OnButtonClick);
            _ownAnim.SetBool("Shown", true);
            _animEvents.OnActionA += OnHideCompletion;
        }

        public void Hide()
        {
            _ownButton.interactable = false;
            _ownAnim.SetBool("Shown", false);
            _cooldownDial.Hide();
        }

        public class Factory : PlaceholderFactory<AbilityControl>
        {
            public AbilityControl Create(Transform parent)
            {
                var newControl = Create();
                newControl.transform.SetParent(parent, false);
                return newControl;
            }
        }
    }
}