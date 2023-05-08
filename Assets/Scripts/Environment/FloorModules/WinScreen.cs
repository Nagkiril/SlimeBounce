using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SlimeBounce.Loading;
using SlimeBounce.UI.PlayerLevel;

namespace SlimeBounce.Environment.FloorModules
{
    public class WinScreen : FloorScreen
    {
        [SerializeField] FloorButton nextButton;
        [SerializeField] FloorButton menuButton;
        [SerializeField] FillPlayerLevelView expBar;
        [SerializeField] PlayerLevelUpNotice levelUpNotice;

        public event Action OnNextLevelPressed;
        public event Action OnMenuPressed;

        protected override void Awake()
        {
            base.Awake();
            nextButton.OnButtonClicked += NextLevelClick;
            menuButton.OnButtonClicked += MenuClicked;
            levelUpNotice.OnExpHandled += OnExpHandled; 
        }

        protected override void MenuAppeared()
        {
            expBar.StartAnimationUpdate();
        }


        private void OnExpHandled()
        {
            menuButton.SetButtonActivity(true);
            nextButton.SetButtonActivity(true);
        }

        private void NextLevelClick()
        {
            DelayedResolution(OnNextLevelPressed);
        }

        private void MenuClicked()
        {
            DelayedResolution(OnMenuPressed);
        }
    }
}