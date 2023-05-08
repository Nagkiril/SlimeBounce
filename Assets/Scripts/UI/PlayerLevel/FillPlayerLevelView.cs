using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SlimeBounce.Player;
using DG.Tweening;

namespace SlimeBounce.UI.PlayerLevel
{
    public class FillPlayerLevelView : MonoBehaviour
    {
        [SerializeField] Image expProgress;
        [SerializeField] TextMeshProUGUI levelText;
        [SerializeField] AnimationType animType;
        [SerializeField] bool holdExpOnFull;
        
        private enum AnimationType
        {
            Instant,
            Automatic,
            Manual
        }

        string _levelTemplate;
        bool _isInitialized;
        Sequence _animationSequence;

        public event Action OnViewUpdated;


        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        private void OnEnable()
        {
            if (_isInitialized)
                UpdateView();
        }

        void Initialize()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                UpdateView();
            }
        }

        void UpdateView()
        {
            if (levelText != null)
            {
                if (string.IsNullOrEmpty(_levelTemplate) && !string.IsNullOrEmpty(levelText.text))
                {
                    _levelTemplate = levelText.text;
                }

                levelText.text = (_levelTemplate != "" ? string.Format(_levelTemplate, PlayerData.PlayerLevel + 1) : $"{PlayerData.PlayerLevel + 1}");
            }

            if (expProgress != null)
            {
                if (animType == AnimationType.Instant)
                {
                    expProgress.fillAmount = ExpController.GetLevelProgress();
                    OnViewUpdated?.Invoke();
                }
                else
                {
                    expProgress.fillAmount = ExpController.LastExpProgress;
                    if (animType == AnimationType.Automatic)
                    {
                        StartAnimationUpdate();
                    }
                }
            }
        }

        public void StartAnimationUpdate()
        {
            float currentProgress = ExpController.GetLevelProgress();
            if (_animationSequence != null)
                _animationSequence.Kill();
            _animationSequence = DOTween.Sequence();
            if (currentProgress > ExpController.LastExpProgress)
            {
                _animationSequence.Append(expProgress.DOFillAmount(currentProgress, 1.2f));
            }
            else
            {
                _animationSequence.Append(expProgress.DOFillAmount(1f, 1.5f));
                if (!holdExpOnFull)
                {
                    _animationSequence.AppendCallback(() => { expProgress.fillAmount = 0; });
                    _animationSequence.Append(expProgress.DOFillAmount(currentProgress, 0.4f));
                }
            }
            _animationSequence.AppendCallback(() => { OnViewUpdated?.Invoke(); });
        }
    }
}