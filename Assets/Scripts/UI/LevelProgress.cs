using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using SlimeBounce.Environment;
using SlimeBounce.Animations.Controllers;
using Zenject;

namespace SlimeBounce.UI
{
    public class LevelProgress : MonoBehaviour
    {
        [SerializeField] private Image _maskImage;
        [SerializeField] private Image _markerImage;
        [SerializeField] private Image _finishImage;
        [SerializeField] private AnimationTriggerController _markerController;
        [SerializeField] private AnimationTriggerController _finishController;
        [SerializeField] private float _maskUpdateDuration;

        private Tween _fillTween;
        private Tween _markerMinTween;
        private Tween _markerMaxTween;

        [Inject]
        private ILevelStateProvider _levelState;
        [Inject]
        private ILivesStateProvider _livesState;

        private void Awake()
        {
            _levelState.OnLevelStarted += ResetFill;
            _levelState.OnLevelProgress += OnProgressUpdated;
            _livesState.OnLivesChanged += OnLivesChanged;
        }

        private void OnDestroy()
        {
            _levelState.OnLevelStarted -= ResetFill;
            _levelState.OnLevelProgress -= OnProgressUpdated;
            _livesState.OnLivesChanged -= OnLivesChanged;
        }

        private void ResetFill()
        {
            StopTweens();
            _maskImage.fillAmount = 0;
            _markerImage.rectTransform.anchorMin = new Vector2(0, 0.5f);
            _markerImage.rectTransform.anchorMax = new Vector2(0, 0.5f);
        }

        private void StopTweens()
        {
            if (_fillTween != null)
                _fillTween.Kill();
            if (_markerMinTween != null)
                _markerMinTween.Kill();
            if (_markerMaxTween != null)
                _markerMaxTween.Kill();
        }

        public void OnLivesChanged()
        {
            if (_livesState.LastLivesDelta < 0)
            {
                _markerController.Trigger();
                _finishController.Trigger();
            }
        }

        public void OnProgressUpdated(int currentProgress, int maxProgress)
        {
            StopTweens();
            float progression = (float)currentProgress / maxProgress;
            Vector2 markerAnchor = new Vector2(progression, 0.5f);
            _fillTween = _maskImage.DOFillAmount(progression, _maskUpdateDuration);
            //Specifically this type of simple marker functionality can be achieved by simply utilizing Sliders, but I wanted to dissect their method of work and emulate it
            _markerMinTween = _markerImage.rectTransform.DOAnchorMin(markerAnchor, _maskUpdateDuration);
            _markerMaxTween = _markerImage.rectTransform.DOAnchorMax(markerAnchor, _maskUpdateDuration);
        }
    }
}