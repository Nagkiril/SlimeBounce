using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SlimeBounce.Player;
using SlimeBounce.UI.Windows;
using DG.Tweening;
using Zenject;

namespace SlimeBounce.UI.PlayerLevel
{
    public class PlayerLevelUpNotice : MonoBehaviour
    {
        [SerializeField] private FillPlayerLevelView _playerExpGauge;
        [SerializeField] private Animator _levelUpAnimator;
        [SerializeField] private AEContainer _levelUpAnimEvents;
        [SerializeField] private bool _openLevelUpWindow;
        [Inject]
        private IPlayerExpManager _playerExp;
        [Inject]
        private IWindowActor _windowActor;

        public event Action OnExpHandled;



        private void Awake()
        {
            if (_levelUpAnimator != null && _levelUpAnimEvents != null)
            {
                _levelUpAnimEvents.OnAnimationDone += OnLevelUpAnimFinish;
            }
            _playerExpGauge.OnViewUpdated += HandleLevelUp;
        }

        private void OnDestroy()
        {
            _playerExpGauge.OnViewUpdated -= HandleLevelUp;
        }

        void OnLevelUpAnimFinish()
        {
            if (_openLevelUpWindow)
            {
                var window = _windowActor.OpenWindow<WindowLevelUp>();
                window.OnWindowClosed += OnLevelNoticeClosed;
            } else
            {
                OnExpHandled?.Invoke();
            }
        }

        void OnLevelNoticeClosed(WindowBase levelWindow)
        {
            OnExpHandled?.Invoke();
        }

        void HandleLevelUp()
        {
            if (_playerExp.GetLevelProgress() <= _playerExp.LastExpProgress)
            {
                if (_levelUpAnimator != null)
                {
                    _levelUpAnimator.SetTrigger("LevelUp");
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