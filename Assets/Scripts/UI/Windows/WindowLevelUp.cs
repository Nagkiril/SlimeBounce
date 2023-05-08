using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SlimeBounce.UI.Windows.LevelUpComponents;
using SlimeBounce.Settings;
using SlimeBounce.Player;
using DG.Tweening;

namespace SlimeBounce.UI.Windows
{
    public class WindowLevelUp : WindowBase
    {
        [Header("Level Up Window Parameters")]
        [SerializeField] RewardView rewardPrefab;
        [SerializeField] Transform rewardContainer;
        [SerializeField] float rewardShowDelay;
        [SerializeField] float rewardNextDelay;
        [SerializeField] float closeBonusDelay;
        [SerializeField] Animator closeAnimator;
        [SerializeField] int rewardsPerScreen;

        private List<RewardView> _activeViews;

        Sequence _rewardShowSequence;


        protected override void Awake()
        {
            base.Awake();
            SetUpRewards();
            ShowActiveRewards();
        }



        void ShowActiveRewards()
        {
            if (_rewardShowSequence == null)
            {
                _rewardShowSequence = DOTween.Sequence();
                _rewardShowSequence.AppendInterval(rewardShowDelay);
                foreach (var reward in _activeViews)
                {
                    _rewardShowSequence.AppendCallback(reward.Show);
                    _rewardShowSequence.AppendInterval(rewardNextDelay);
                }
                _rewardShowSequence.AppendInterval(closeBonusDelay);
                _rewardShowSequence.AppendCallback(AllowClosure);
            }
        }

        void AllowClosure()
        {
            closeAnimator.SetBool("Shown", true);
        }

        void SetUpRewards()
        {
            _activeViews = new List<RewardView>();
            var newSlimes = PlayerLevelSettings.GetSlimeLevelUnlocks(PlayerData.PlayerLevel);
            float currencyDelta = GetCurrencyMultiplier() - GetCurrencyMultiplier(PlayerData.PlayerLevel - 1);
            float slimeSpeedDelta = GetSpeedMultiplier() - GetSpeedMultiplier(PlayerData.PlayerLevel - 1);
            float delayReductionDelta = GetSpawnDelayReduction() - GetSpawnDelayReduction(PlayerData.PlayerLevel - 1);


            foreach (var slimeType in newSlimes)
            {
                var slimeView = Instantiate(rewardPrefab, rewardContainer);
                slimeView.Initialize(PlayerRewardViewSettings.GetSlimeReward(slimeType));
                _activeViews.Add(slimeView);
            }

            SpawnDeltaReward(currencyDelta, PlayerRewardViewSettings.GetCurrencyReward);
            SpawnDeltaReward(slimeSpeedDelta, PlayerRewardViewSettings.GetSpeedReward);
            SpawnDeltaReward(delayReductionDelta, PlayerRewardViewSettings.GetSpawnReward);
        }

        void SpawnDeltaReward(float numDelta, Func<RewardViewData> rewardCallback)
        {
            //Should expand the window to allow more than rewardsPerScreen amount of views
            if (numDelta > 0 && _activeViews.Count < rewardsPerScreen)
            {
                var view = Instantiate(rewardPrefab, rewardContainer);
                //If we need to show exact delta on reward window, we can easily modify Description with a template and use that template here, before initialization
                view.Initialize(rewardCallback.Invoke());
                _activeViews.Add(view);
            }
        }

        float GetCurrencyMultiplier(int playerLevel = -1)
        {
            if (playerLevel == -1)
                playerLevel = PlayerData.PlayerLevel;
            return PlayerLevelSettings.GetCurrencyMultiplier(playerLevel);
        }

        float GetSpeedMultiplier(int playerLevel = -1)
        {
            if (playerLevel == -1)
                playerLevel = PlayerData.PlayerLevel;
            return PlayerLevelSettings.GetSpeedMultiplier(playerLevel);
        }
        float GetSpawnDelayReduction(int playerLevel = -1)
        {
            if (playerLevel == -1)
                playerLevel = PlayerData.PlayerLevel;
            return PlayerLevelSettings.GetSpawnDelayReduction(playerLevel);
        }
    }
}