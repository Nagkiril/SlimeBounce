using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SlimeBounce.Player;
using SlimeBounce.Animations.Controllers;

namespace SlimeBounce.UI.Count
{
    public class CoinCounter : MonoBehaviour
    {
        [SerializeField] private Transform _flightDestination;
        [SerializeField] private TextCounter _countText;
        [SerializeField] private AnimationTriggerController _showController;

        public event Action OnCountFinished;
        
        private void Awake()
        {
            _countText.OnAnimationDone += OnCoinsAccounted;
            CountCoins();
        }

        public void OnCoinsAccounted()
        {
            OnCountFinished?.Invoke();
        }

        public void CountCoins(float countDuration = 0.5f)
        {
            _countText.CountTo(PlayerData.Currency, countDuration);
        }

        public void BounceIcon()
        {
            _showController.Trigger();
        }

        public Vector3 GetFlightPosition() => _flightDestination.position;

    }
}