using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Environment;
using SlimeBounce.UI.Count;

namespace SlimeBounce.Player
{
    public class CoinController : MonoBehaviour
    {
        [SerializeField] CoinCounter coinUI;
        [SerializeField] FlyingCoin flyingCoinPrefab;
        [SerializeField] float coinCountDuration;
        [SerializeField] float coinFlightDuration;

        List<FlyingCoin> _flyingCoins;
        static CoinController _instance;

        public bool CoinsAnimated => _flyingCoins.Count > 0;

        const int MIN_FLIGHT_SPAWNS = 1;
        const int MAX_FLIGHT_SPAWNS = 15;
        const int COINS_PER_FLIGHT = 5;


        // Start is called before the first frame update
        void Start()
        {
            _flyingCoins = new List<FlyingCoin>();

            _instance = this;
        }

        public static bool ChangeCoins(int delta)
        {
            if (delta > 0 || PlayerData.Currency >= delta)
            {
                PlayerData.Currency += delta;
                if (!_instance.CoinsAnimated)
                {
                    _instance.UpdateCoinCounter();
                }
                return true;
            } else
            {
                return false;
            }
        } 

        public static void ChangeCoins(int value, Vector3 worldPosition)
        {
            _instance.SpawnCoinsAt(GetCoinSpawnCount(value), worldPosition);
            ChangeCoins(value);
        }

        private static int GetCoinSpawnCount(int value)
        {
            var coinsAmount = value / COINS_PER_FLIGHT;
            coinsAmount = Mathf.Max(Mathf.Min(coinsAmount, MAX_FLIGHT_SPAWNS), MIN_FLIGHT_SPAWNS);
            return coinsAmount;
        }

        private void UpdateCoinCounter()
        {
            coinUI.CountCoins(coinCountDuration);
        }

        private void OnCoinFlightEnd(FlyingCoin coin)
        {
            _flyingCoins.Remove(coin);
            coinUI.BounceIcon();
            UpdateCoinCounter();
        }

        private void SpawnCoinsAt(int coinCount, Vector3 worldPosition)
        {
            var spawnScreenPosition = CameraController.Instance.GetScreenFromWorld(worldPosition);
            for (var i = 0; i < coinCount; i++)
            {
                var newCoin = Instantiate(flyingCoinPrefab, transform);
                newCoin.SetAnchor(spawnScreenPosition);
                newCoin.OnFlightFinished += OnCoinFlightEnd;
                newCoin.FlyToLocal(transform.InverseTransformPoint(coinUI.GetFlightPosition()), coinFlightDuration);
                _flyingCoins.Add(newCoin);
            }
        }
    }
}