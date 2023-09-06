using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SlimeBounce.Loading;
using SlimeBounce.UI.Windows;

namespace SlimeBounce.Environment.FloorModules
{
    public class LobbyScreen : FloorScreen
    {
        [SerializeField] private FloorButton _startButton;
        [SerializeField] private FloorButton _shopButton;

        public event Action OnLevelStartPressed;
        public event Action OnShopPressed;


        protected override void Awake()
        {
            base.Awake();
            _startButton.OnButtonClicked += StartLevelClick;
            _shopButton.OnButtonClicked += OpenShopClick;
        }

        private void StartLevelClick()
        {
            DelayedResolution(OnLevelStartPressed);
        }

        private void OpenShopClick()
        {
            DelayedResolution(OnShopPressed, false);
        }

        protected override void MenuAppeared()
        {
            _startButton.SetButtonActivity(true);
            _shopButton.SetButtonActivity(true);
        }
    }
}