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
        [SerializeField] private FloorButton _nextButton;
        [SerializeField] private FloorButton _menuButton;
        [SerializeField] private FillPlayerLevelView _expBar;
        [SerializeField] private PlayerLevelUpNotice _levelUpNotice;

        public event Action OnNextLevelPressed;
        public event Action OnMenuPressed;

        protected override void Awake()
        {
            base.Awake();
            _nextButton.OnButtonClicked += NextLevelClick;
            _menuButton.OnButtonClicked += MenuClicked;
            _levelUpNotice.OnExpHandled += OnExpHandled; 
        }

        protected override void MenuAppeared()
        {
            _expBar.StartAnimationUpdate();
        }


        private void OnExpHandled()
        {
            _menuButton.SetButtonActivity(true);
            _nextButton.SetButtonActivity(true);
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