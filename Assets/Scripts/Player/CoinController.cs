using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Environment;
using SlimeBounce.UI.Count;

namespace SlimeBounce.Player
{
    public class CoinController : MonoBehaviour, ICoinActor
    {
        [SerializeField] private CoinCounter _coinUI;
        [SerializeField] private FlyingCoin _flyingCoinPrefab;
        [SerializeField] private float _coinCountDuration;
        [SerializeField] private float _coinFlightDuration;
        private const int MIN_FLIGHT_SPAWNS = 1;
        private const int MAX_FLIGHT_SPAWNS = 15;
        private const int COINS_PER_FLIGHT = 15;
        private List<FlyingCoin> _flyingCoins;

        public bool CoinsAnimated => _flyingCoins.Count > 0;

        private void Awake()
        {
            _flyingCoins = new List<FlyingCoin>();
        }

        private int GetCoinSpawnCount(int value)
        {
            var coinsAmount = value / COINS_PER_FLIGHT;
            coinsAmount = Mathf.Max(Mathf.Min(coinsAmount, MAX_FLIGHT_SPAWNS), MIN_FLIGHT_SPAWNS);
            return coinsAmount;
        }

        private void UpdateCoinCounter()
        {
            _coinUI.CountCoins(_coinCountDuration);
        }

        private void OnCoinFlightEnd(FlyingCoin coin)
        {
            _flyingCoins.Remove(coin);
            _coinUI.BounceIcon();
            UpdateCoinCounter();
        }

        private void SpawnCoinsAt(int coinCount, Vector3 worldPosition)
        {
            var spawnScreenPosition = CameraController.Instance.GetScreenFromWorld(worldPosition);
            for (var i = 0; i < coinCount; i++)
            {
                var newCoin = Instantiate(_flyingCoinPrefab, transform);
                newCoin.SetAnchor(spawnScreenPosition);
                newCoin.OnFlightFinished += OnCoinFlightEnd;
                newCoin.FlyToLocal(transform.InverseTransformPoint(_coinUI.GetFlightPosition()), _coinFlightDuration);
                _flyingCoins.Add(newCoin);
            }
        }

        public bool ChangeCoins(int delta)
        {
            if (delta > 0 || (PlayerData.Currency + delta) >= 0)
            {
                PlayerData.Currency += delta;
                if (!CoinsAnimated)
                {
                    UpdateCoinCounter();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ChangeCoins(int value, Vector3 worldPosition)
        {
            bool couldChangeCoins = ChangeCoins(value);
            if (couldChangeCoins)
            {
                SpawnCoinsAt(GetCoinSpawnCount(value), worldPosition);
            }
            return couldChangeCoins;
        }
    }
}