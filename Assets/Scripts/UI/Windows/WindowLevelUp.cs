using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SlimeBounce.Animations.Controllers;
using SlimeBounce.UI.Windows.LevelUpComponents;
using SlimeBounce.UI.Settings;
using SlimeBounce.Settings;
using SlimeBounce.Player;
using SlimeBounce.Player.Settings;
using DG.Tweening;
using Zenject;

namespace SlimeBounce.UI.Windows
{
    public class WindowLevelUp : WindowBase
    {
        [Header("Level Up Window Parameters")]
        [SerializeField] private RewardView _rewardPrefab;
        [SerializeField] private Transform _rewardContainer;
        [SerializeField] private float _rewardShowDelay;
        [SerializeField] private float _rewardNextDelay;
        [SerializeField] private float _closeBonusDelay;
        [SerializeField] private int _rewardsPerScreen;
        [SerializeField] private ShowHideActiveController _closeShowController;
        private List<RewardView> _activeViews;
        private Sequence _rewardShowSequence;

        [Inject]
        private IPlayerLevelSettings _playerLevelSettings;
        [Inject]
        private IPlayerRewardViewSettings _rewardViewSettings;

        protected override void Awake()
        {
            base.Awake();
            SetUpRewards();
            ShowActiveRewards();
            _closeShowController.Hide(false);
        }

        private void ShowActiveRewards()
        {
            if (_rewardShowSequence == null)
            {
                _rewardShowSequence = DOTween.Sequence();
                _rewardShowSequence.AppendInterval(_rewardShowDelay);
                foreach (var reward in _activeViews)
                {
                    _rewardShowSequence.AppendCallback(reward.Show);
                    _rewardShowSequence.AppendInterval(_rewardNextDelay);
                }
                _rewardShowSequence.AppendInterval(_closeBonusDelay);
                _rewardShowSequence.AppendCallback(AllowClosure);
            }
        }

        private void AllowClosure()
        {
            _closeShowController.Show();
        }

        private void SetUpRewards()
        {
            _activeViews = new List<RewardView>();
            var newSlimes = _playerLevelSettings.GetSlimeLevelUnlocks(PlayerData.PlayerLevel);
            float currencyDelta = GetCurrencyMultiplier() - GetCurrencyMultiplier(PlayerData.PlayerLevel - 1);
            float slimeSpeedDelta = GetSpeedMultiplier() - GetSpeedMultiplier(PlayerData.PlayerLevel - 1);
            float delayReductionDelta = GetSpawnDelayReduction() - GetSpawnDelayReduction(PlayerData.PlayerLevel - 1);


            foreach (var slimeType in newSlimes)
            {
                var slimeView = Instantiate(_rewardPrefab, _rewardContainer);
                slimeView.Initialize(_rewardViewSettings.GetSlimeReward(slimeType));
                _activeViews.Add(slimeView);
            }

            SpawnDeltaReward(currencyDelta, _rewardViewSettings.GetCurrencyReward);
            SpawnDeltaReward(slimeSpeedDelta, _rewardViewSettings.GetSpeedReward);
            SpawnDeltaReward(delayReductionDelta, _rewardViewSettings.GetSpawnReward);
        }

        private void SpawnDeltaReward(float numDelta, Func<RewardViewData> rewardCallback)
        {
            //Should expand the window to allow more than rewardsPerScreen amount of views
            if (numDelta > 0 && _activeViews.Count < _rewardsPerScreen)
            {
                var view = Instantiate(_rewardPrefab, _rewardContainer);
                //If we need to show exact delta on reward window, we can easily modify Description with a template and use that template here, before initialization
                view.Initialize(rewardCallback.Invoke());
                _activeViews.Add(view);
            }
        }

        private float GetCurrencyMultiplier(int playerLevel = -1)
        {
            if (playerLevel == -1)
                playerLevel = PlayerData.PlayerLevel;
            return _playerLevelSettings.GetCurrencyMultiplier(playerLevel);
        }

        private float GetSpeedMultiplier(int playerLevel = -1)
        {
            if (playerLevel == -1)
                playerLevel = PlayerData.PlayerLevel;
            return _playerLevelSettings.GetSpeedMultiplier(playerLevel);
        }

        private float GetSpawnDelayReduction(int playerLevel = -1)
        {
            if (playerLevel == -1)
                playerLevel = PlayerData.PlayerLevel;
            return _playerLevelSettings.GetSpawnDelayReduction(playerLevel);
        }
    }
}