using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SlimeBounce.Loading;
using SlimeBounce.UI.PlayerLevel;

namespace SlimeBounce.Environment.FloorModules
{
    public class LoseScreen : FloorScreen
    {
        [SerializeField] private FloorButton _retryButton;
        [SerializeField] private FloorButton _menuButton;
        [SerializeField] private FillPlayerLevelView _expBar;
        [SerializeField] private PlayerLevelUpNotice _levelUpNotice;

        public event Action OnRetryPressed;
        public event Action OnMenuPressed;

        protected override void Awake()
        {
            base.Awake();
            _retryButton.OnButtonClicked += OnRetryClick;
            _menuButton.OnButtonClicked += OnMenuClick;
            _levelUpNotice.OnExpHandled += OnExpHandled;
        }

        private void OnExpHandled()
        {
            _retryButton.SetButtonActivity(true);
            _menuButton.SetButtonActivity(true);
        }

        protected override void MenuAppeared()
        {
            _expBar.StartAnimationUpdate();
        }

        protected void OnRetryClick()
        {
            DelayedResolution(OnRetryPressed);
        }

        protected void OnMenuClick()
        {
            DelayedResolution(OnMenuPressed);
        }
    }
}