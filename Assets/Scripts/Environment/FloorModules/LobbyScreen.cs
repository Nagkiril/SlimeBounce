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
        [SerializeField] FloorButton startButton;
        [SerializeField] FloorButton shopButton;

        public event Action OnLevelStartPressed;
        public event Action OnShopPressed;


        protected override void Awake()
        {
            base.Awake();
            startButton.OnButtonClicked += StartLevelClick;
            shopButton.OnButtonClicked += OpenShopClick;
        }

        protected override void MenuAppeared()
        {
            startButton.SetButtonActivity(true);
            shopButton.SetButtonActivity(true);
        }

        private void StartLevelClick()
        {
            DelayedResolution(OnLevelStartPressed);
        }

        private void OpenShopClick()
        {
            DelayedResolution(OnShopPressed, false);
        }
    }
}