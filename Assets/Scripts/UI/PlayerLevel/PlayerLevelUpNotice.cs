using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SlimeBounce.Player;
using SlimeBounce.UI.Windows;
using DG.Tweening;

namespace SlimeBounce.UI.PlayerLevel
{
    public class PlayerLevelUpNotice : MonoBehaviour
    {
        [SerializeField] FillPlayerLevelView playerExpGauge;
        [SerializeField] Animator levelUpAnimator;
        [SerializeField] AEContainer levelUpAnimEvents;
        [SerializeField] bool openLevelUpWindow;

        public event Action OnExpHandled;

        private void Awake()
        {
            if (levelUpAnimator != null && levelUpAnimEvents != null)
            {
                levelUpAnimEvents.OnAnimationDone += OnLevelUpAnimFinish;
            }
            playerExpGauge.OnViewUpdated += HandleLevelUp;
        }

        private void OnDestroy()
        {
            playerExpGauge.OnViewUpdated -= HandleLevelUp;
        }

        void OnLevelUpAnimFinish()
        {
            if (openLevelUpWindow)
            {
                var window = WindowController.OpenWindow<WindowLevelUp>();
                window.OnWindowClosed += OnLevelNoticeClosed;
            } else
            {
                OnExpHandled?.Invoke();
            }
        }

        void OnLevelNoticeClosed()
        {
            OnExpHandled?.Invoke();
        }

        void HandleLevelUp()
        {
            if (ExpController.GetLevelProgress() <= ExpController.LastExpProgress)
            {
                if (levelUpAnimator != null)
                {
                    levelUpAnimator.SetTrigger("LevelUp");
                } else
                {
                    OnLevelUpAnimFinish();
                }
            }
            else
            {
                OnExpHandled?.Invoke();
            }
        }
    }
}