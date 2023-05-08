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
        [SerializeField] FloorButton retryButton;
        [SerializeField] FillPlayerLevelView expBar;
        [SerializeField] PlayerLevelUpNotice levelUpNotice;

        public event Action OnRetryPressed;

        protected override void Awake()
        {
            base.Awake();
            retryButton.OnButtonClicked += RetryClick;
            levelUpNotice.OnExpHandled += OnExpHandled;
        }

        protected override void MenuAppeared()
        {
            expBar.StartAnimationUpdate();
        }

        private void OnExpHandled()
        {
            retryButton.SetButtonActivity(true);
        }

        public void RetryClick()
        {
            DelayedResolution(OnRetryPressed);
        }
    }
}