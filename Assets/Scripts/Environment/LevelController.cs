using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Loading;
using SlimeBounce.Settings;
using SlimeBounce.Slime;
using SlimeBounce.Player;
using SlimeBounce.UI.Windows;
using DG.Tweening;

namespace SlimeBounce.Environment
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] SlimeSpawner spawner;
        [SerializeField] SlimeDespawner despawner;
        [SerializeField] float restartSceneDelay;

        public static event Action OnLevelStarted;
        public static event Action OnLivesChanged;
        public static event Action<int, int> OnLevelProgress;
        public static event Action<bool> OnLevelEnded;
        public static event Action OnLobbyEntered;

        public static bool IsLevelInProgress { get; private set; }
        public static int Lives { get; private set; }
        public static bool HasCooldownAbilities { get; private set; }

        //Not a big fan of keeping this data, but I don't want to modify existing OnLivesChanged signature
        public static int LastLivesDelta { get; private set; }



        int _slimesInLevel;

        void Start()
        {
            //We don't have abilities yet, so this flag is set for debug purposes
            HasCooldownAbilities = true;

            despawner.OnSlimeReached += OnSlimeEscaped;
            spawner.OnSlimeSpawned += OnSlimeSpawned;
            Floor.OnLevelStartPressed += StartLevel;
            Floor.OnNextLevelPressed += StartLevel;
            Floor.OnRetryPressed += StartLevel;
            Floor.OnShopPressed += OpenShop;
            Floor.OnMenuPressed += EnterLobby;
        }

        private void OnDestroy()
        {
            despawner.OnSlimeReached -= OnSlimeEscaped;
            spawner.OnSlimeSpawned -= OnSlimeSpawned;
            Floor.OnLevelStartPressed -= StartLevel;
            Floor.OnNextLevelPressed -= StartLevel;
            Floor.OnRetryPressed -= StartLevel;
            Floor.OnShopPressed -= OpenShop;
            Floor.OnMenuPressed -= EnterLobby;
        }

        void StartLevel()
        {
            SetLives(LevelSettings.GetDefaultLives() + (int)UpgradeController.GetUpgradeValue(UpgradeType.Lives));
            var playerLevel = PlayerData.PlayerLevel;
            var slimeSpeed = PlayerLevelSettings.GetSpeedMultiplier(playerLevel) * (1f - UpgradeController.GetUpgradeValue(UpgradeType.Slowdown) * 0.01f);
            spawner.StartSpawning(PlayerLevelSettings.GetUnlockedSlimes(playerLevel), slimeSpeed, PlayerLevelSettings.GetSpawnDelayReduction(playerLevel));
            IsLevelInProgress = true;
            OnLevelStarted?.Invoke();
        }

        void EnterLobby()
        {
            OnLobbyEntered?.Invoke();
        }

        void OpenShop()
        {
            WindowController.OpenWindow<WindowShop>();
        }

        void EndLevel(bool isWin)
        {
            if (IsLevelInProgress)
            {
                IsLevelInProgress = false;
                Debug.Log($"Level {(isWin ? "won" : "lost")}!");
                spawner.StopSpawning();
                float appliedRewardRatio = ((float)GetLevelSlimesLeft() / spawner.SlimesTotal * 0.5f) + (isWin ? 0.5f : 0);
                if (isWin)
                {
                    appliedRewardRatio += (0.01f * UpgradeController.GetUpgradeValue(UpgradeType.Investment));
                }
                CoinController.ChangeCoins(Mathf.FloorToInt(appliedRewardRatio * LevelSettings.GetLevelReward() * PlayerLevelSettings.GetCurrencyMultiplier(PlayerData.PlayerLevel)), transform.position);
                OnLevelEnded?.Invoke(isWin);
                if (isWin)
                {
                    ExpController.AddExp(LevelSettings.GetRewardExp());
                    ProgressToNextLevel();
                }
            }
        }

        void ProgressToNextLevel()
        {
            PlayerData.PassedLevels++;
            var predefinedLevels = LevelSettings.GetLevelsAmount();
            if (PlayerData.PassedLevels < predefinedLevels)
            {
                PlayerData.CurrentLevel = PlayerData.PassedLevels;
            } else
            {
                List<int> availableLevels = new List<int>();
                for (var i = 0; i < predefinedLevels; i++)
                {
                    availableLevels.Add(i);
                }
                availableLevels.Remove(PlayerData.CurrentLevel);
                PlayerData.CurrentLevel = availableLevels[UnityEngine.Random.Range(0, availableLevels.Count)];
            }
        }

        private int GetLevelSlimesLeft()
        {
            return spawner.SlimesTotal - spawner.SlimesUnspawned - _slimesInLevel;
        }

        private void OnSlimeEscaped(SlimeCore escapedSlime)
        {
            SetLives(Lives - escapedSlime.GetLifeDamage());
            CheckLevelEndTriggers();
        }

        private void OnSlimeRemoved()
        {
            AccountSlimeRemoved();
            CheckLevelEndTriggers();
        }

        void CheckLevelEndTriggers()
        {
            CheckLossTrigger();
            CheckVictoryTrigger();
        }

        void CheckLossTrigger()
        {
            if (IsLevelInProgress && Lives == 0)
            {
                EndLevel(false);
            }
        }

        void CheckVictoryTrigger()
        {
            if (IsLevelInProgress && spawner.SlimesUnspawned == 0 && _slimesInLevel == 0)
            {
                EndLevel(true);
            }
        }

        public static void RechargeAbilities()
        {
            Debug.Log("Abilities recharged by 1 segment!");
        }


        private void OnSlimeSpawned(SlimeCore newSlime)
        {
            newSlime.OnSlimeDestroyed += OnSlimeRemoved;
            _slimesInLevel++;
        }

        private void SetLives(int newLives)
        {
            if (newLives < 0)
                newLives = 0;
            if (Lives != newLives)
            {
                LastLivesDelta = newLives - Lives;
                Lives = newLives;
                OnLivesChanged?.Invoke();
            }
        }

        private void AccountSlimeRemoved()
        {
            _slimesInLevel--;
            NotifyLevelProgress();
        }

        private void NotifyLevelProgress()
        {
            OnLevelProgress?.Invoke(GetLevelSlimesLeft(), spawner.SlimesTotal);
        }
    }
}