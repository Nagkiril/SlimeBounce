using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using SlimeBounce.Environment;

namespace SlimeBounce.UI
{
    public class LevelProgress : MonoBehaviour
    {
        [SerializeField] Image maskImage;
        [SerializeField] Image markerImage;
        [SerializeField] Image finishImage;
        [SerializeField] Animator markerAnimator;
        [SerializeField] Animator finishAnimator;
        [SerializeField] float maskUpdateDuration;

        Tween _fillTween;
        Tween _markerMinTween;
        Tween _markerMaxTween;

        // Start is called before the first frame update
        void Awake()
        {
            LevelController.OnLevelStarted += ResetFill;
            LevelController.OnLivesChanged += OnLivesChanged;
            LevelController.OnLevelProgress += OnProgressUpdated;
        }

        private void OnDestroy()
        {
            LevelController.OnLevelStarted -= ResetFill;
            LevelController.OnLivesChanged -= OnLivesChanged;
            LevelController.OnLevelProgress -= OnProgressUpdated;
        }

        public void OnLivesChanged()
        {
            if (LevelController.LastLivesDelta < 0)
            {
                markerAnimator.SetTrigger("DamageTaken");
                finishAnimator.SetTrigger("DamageTaken");
            }
        }

        void ResetFill()
        {
            StopTweens();
            maskImage.fillAmount = 0;
            markerImage.rectTransform.anchorMin = new Vector2(0, 0.5f);
            markerImage.rectTransform.anchorMax = new Vector2(0, 0.5f);
        }

        public void OnProgressUpdated(int currentProgress, int maxProgress)
        {
            StopTweens();
            float progression = (float)currentProgress / maxProgress;
            Vector2 markerAnchor = new Vector2(progression, 0.5f);
            _fillTween = maskImage.DOFillAmount(progression, maskUpdateDuration);
            //Specifically this type of simple marker functionality can be achieved by simply utilizing Sliders, but I wanted to dissect their method of work and emulate it
            _markerMinTween = markerImage.rectTransform.DOAnchorMin(markerAnchor, maskUpdateDuration);
            _markerMaxTween = markerImage.rectTransform.DOAnchorMax(markerAnchor, maskUpdateDuration);
        }

        void StopTweens()
        {
            if (_fillTween != null)
                _fillTween.Kill();
            if (_markerMinTween != null)
                _markerMinTween.Kill();
            if (_markerMaxTween != null)
                _markerMaxTween.Kill();
        }
    }
}