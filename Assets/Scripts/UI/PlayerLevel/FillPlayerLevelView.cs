using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SlimeBounce.Player;
using DG.Tweening;
using Zenject;

namespace SlimeBounce.UI.PlayerLevel
{
    public class FillPlayerLevelView : MonoBehaviour
    {
        [SerializeField] private Image _expProgress;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private AnimationType _animType;
        [SerializeField] private bool _holdExpOnFull;
        
        private enum AnimationType
        {
            Instant,
            Automatic,
            Manual
        }

        private string _levelTemplate;
        private bool _isInitialized;
        private Sequence _animationSequence;

        [Inject]
        private IPlayerExpManager _playerExp;

        public event Action OnViewUpdated;


        private void Start()
        {
            Initialize();
        }

        private void OnEnable()
        {
            if (_isInitialized)
                UpdateView();
        }

        private void Initialize()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                UpdateView();
            }
        }

        private void UpdateView()
        {
            if (_levelText != null)
            {
                if (string.IsNullOrEmpty(_levelTemplate) && !string.IsNullOrEmpty(_levelText.text))
                {
                    _levelTemplate = _levelText.text;
                }

                _levelText.text = (_levelTemplate != "" ? string.Format(_levelTemplate, PlayerData.PlayerLevel + 1) : $"{PlayerData.PlayerLevel + 1}");
            }

            if (_expProgress != null)
            {
                if (_animType == AnimationType.Instant)
                {
                    _expProgress.fillAmount = _playerExp.GetLevelProgress();
                    OnViewUpdated?.Invoke();
                }
                else
                {
                    _expProgress.fillAmount = _playerExp.LastExpProgress;
                    if (_animType == AnimationType.Automatic)
                    {
                        StartAnimationUpdate();
                    }
                }
            }
        }

        public void StartAnimationUpdate()
        {
            float currentProgress = _playerExp.GetLevelProgress();
            if (_animationSequence != null)
                _animationSequence.Kill();
            _animationSequence = DOTween.Sequence();
            if (currentProgress > _playerExp.LastExpProgress)
            {
                _animationSequence.Append(_expProgress.DOFillAmount(currentProgress, 0.7f));
            }
            else
            {
                _animationSequence.Append(_expProgress.DOFillAmount(1f, 0.9f));
                if (!_holdExpOnFull)
                {
                    _animationSequence.AppendCallback(() => { _expProgress.fillAmount = 0; });
                    _animationSequence.Append(_expProgress.DOFillAmount(currentProgress, 0.2f));
                }
            }
            _animationSequence.AppendCallback(() => { OnViewUpdated?.Invoke(); });
        }
    }
}