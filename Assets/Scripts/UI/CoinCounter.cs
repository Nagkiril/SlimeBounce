using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SlimeBounce.Player;

namespace SlimeBounce.UI.Count
{
    public class CoinCounter : MonoBehaviour
    {
        [SerializeField] Transform flightDestination;
        [SerializeField] TextCounter countText;
        [SerializeField] Animator iconAnim;

        public event Action OnCountFinished;
        
        private void Awake()
        {
            countText.OnAnimationDone += OnCoinsAccounted;
            CountCoins();
        }

        public void OnCoinsAccounted()
        {
            OnCountFinished?.Invoke();
        }

        public void CountCoins(float countDuration = 0.5f)
        {
            countText.CountTo(PlayerData.Currency, countDuration);
        }

        private void DisperseParticles()
        {

        }

        public void BounceIcon()
        {
            iconAnim.SetTrigger("Bounce");
        }

        public Vector3 GetFlightPosition() => flightDestination.position;

    }
}