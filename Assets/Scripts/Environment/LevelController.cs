using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Loading;
using SlimeBounce.Environment.Settings;
using SlimeBounce.Settings;
using SlimeBounce.Slime;
using SlimeBounce.Player;
using SlimeBounce.Player.Settings;
using SlimeBounce.UI.Windows;
using DG.Tweening;
using Zenject;

namespace SlimeBounce.Environment
{
    //We could uh... defenitely split this into multiple different classes; I'm not really going to do that because development cycle winds down for this project, and this old man was among the first scripts for this game!
    //I prefer to have my code within 100-150 lines of code, ~200 we have here isn't horrible but usually is a good sign that some things aren't right
    public class LevelController : MonoBehaviour, ILevelStateProvider, ILivesStateProvider
    {
        [SerializeField] private SlimeSpawner _spawner;
        [SerializeField] private SlimeDespawner _despawner;
        [SerializeField] private float _restartSceneDelay;
        private int _slimesInLevel;

        //We could combine them all with an inject method instead - which is something I would normally prefer inject methods - but for consistency across the project I'm injecting the same way as everywhere else
        [Inject]
        private ILevelSettings _levelSettings;
        [Inject]
        private IPlayerLevelSettings _playerLevelSettings;
        [Inject]
        private IUpgradeDataProvider _upgradeData;
        [Inject]
        private IPlayerExpManager _expManager;
        [Inject]
        private IWindowActor _windowActor;
        [Inject]
        private IFloorScreenEvents _floorEvents;
        [Inject]
        private IGameSetupHandler _gameSetup;
        [Inject]
        private ICoinActor _coinActor;

        public event Action OnLevelStarted;
        public event Action OnLivesChanged;
        public event Action<int, int> OnLevelProgress;
        public event Action<bool> OnLevelEnded;
        public event Action OnLobbyEntered;

        public bool IsLevelInProgress { get; private set; }
        public int Lives { get; private set; }

        //Not a big fan of keeping this data, but I don't want to modify existing OnLivesChanged signature
        public int LastLivesDelta { get; private set; }


        private void Awake()
        {
            _gameSetup.RunSetup();
        }

        private void Start()
        {
            _despawner.OnSlimeReached += OnSlimeEscaped;
            _spawner.OnSlimeSpawned += OnSlimeSpawned;
            _floorEvents.OnLevelStartPressed += StartLevel;
            _floorEvents.OnNextLevelPressed += StartLevel;
            _floorEvents.OnRetryPressed += StartLevel;
            _floorEvents.OnShopPressed += OpenShop;
            _floorEvents.OnMenuPressed += EnterLobby;
        }

        private void OnDestroy()
        {
            _despawner.OnSlimeReached -= OnSlimeEscaped;
            _spawner.OnSlimeSpawned -= OnSlimeSpawned;
            _floorEvents.OnLevelStartPressed -= StartLevel;
            _floorEvents.OnNextLevelPressed -= StartLevel;
            _floorEvents.OnRetryPressed -= StartLevel;
            _floorEvents.OnShopPressed -= OpenShop;
            _floorEvents.OnMenuPressed -= EnterLobby;
        }

        private void StartLevel()
        {
            SetLives(_levelSettings.GetDefaultLives() + (int)_upgradeData.GetUpgradeValues(UpgradeType.Lives)[0]);
            var playerLevel = PlayerData.PlayerLevel;
            var slimeSpeed = _playerLevelSettings.GetSpeedMultiplier(playerLevel) * (1f - _upgradeData.GetUpgradeValues(UpgradeType.Slowdown)[0] * 0.01f);
            _spawner.StartSpawning(_playerLevelSettings.GetUnlockedSlimes(playerLevel), slimeSpeed, _playerLevelSettings.GetSpawnDelayReduction(playerLevel));
            IsLevelInProgress = true;
            OnLevelStarted?.Invoke();
        }

        private void EnterLobby()
        {
            OnLobbyEntered?.Invoke();
        }

        private void OpenShop()
        {
            _windowActor.OpenWindow<WindowShop>();
        }

        private void EndLevel(bool isWin)
        {
            if (IsLevelInProgress)
            {
                IsLevelInProgress = false;
                _spawner.StopSpawning();
                float appliedRewardRatio = ((float)GetLevelSlimesLeft() / _spawner.SlimesTotal * 0.5f) + (isWin ? 0.5f : 0);
                if (isWin)
                {
                    appliedRewardRatio += (0.01f * _upgradeData.GetUpgradeValues(UpgradeType.Investment)[0]);
                }
                _coinActor.ChangeCoins(Mathf.FloorToInt(appliedRewardRatio * _levelSettings.GetLevelReward() * _playerLevelSettings.GetCurrencyMultiplier(PlayerData.PlayerLevel)), transform.position);
                OnLevelEnded?.Invoke(isWin);
                if (isWin)
                {
                    _expManager.AddExp(_levelSettings.GetRewardExp());
                    ProgressToNextLevel();
                }
            }
        }

        private void ProgressToNextLevel()
        {
            PlayerData.PassedLevels++;
            var predefinedLevels = _levelSettings.GetLevelsAmount();
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
            return _spawner.SlimesTotal - _spawner.SlimesUnspawned - _slimesInLevel;
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

        private void CheckLevelEndTriggers()
        {
            CheckLossTrigger();
            CheckVictoryTrigger();
        }

        private void CheckLossTrigger()
        {
            if (IsLevelInProgress && Lives == 0)
            {
                EndLevel(false);
            }
        }

        private void CheckVictoryTrigger()
        {
            if (IsLevelInProgress && _spawner.SlimesUnspawned == 0 && _slimesInLevel == 0)
            {
                EndLevel(true);
            }
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
            OnLevelProgress?.Invoke(GetLevelSlimesLeft(), _spawner.SlimesTotal);
        }
    }
}