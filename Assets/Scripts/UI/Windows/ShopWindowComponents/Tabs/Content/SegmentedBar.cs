using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SlimeBounce.UI.Windows.ShopComponents.Tabs.Content.BarComponents;
using DG.Tweening;

namespace SlimeBounce.UI.Windows.ShopComponents.Tabs.Content
{
    public class SegmentedBar : MonoBehaviour
    {
        [SerializeField] BarSegment segmentPrefab;
        [SerializeField] Transform segmentContainer;
        [SerializeField] Image maskImage;
        [SerializeField] Color regularColor;
        [SerializeField] Color fullColor;
        [SerializeField] float fillTargetOffset;
        [SerializeField] float fillSourceOffset;
        [SerializeField] float fillDuration;

        public event Action OnBarUpgradeStarted;
        public event Action OnBarChanged;

        //This looks very much like an interface, I think it should be separated\made into a separate component?
        public int MaxProgress { get; private set; }
        public int Progress { get; private set; }

        bool _isInitialized;
        List<BarSegment> _segments;
        Tween _fillWidthTween;
        Tween _fillAnchorTween;
        Tween _fillColorTween;
        Sequence _segmentAnimationSequence;
        float _fullWidth;

        public void SetMaxProgress(int maxProgress)
        {
            MaxProgress = maxProgress;
            ResetSegments();
            InitializeFillParameters();
        }

        public void SetProgress(int newProgress)
        {
            Progress = Mathf.Min(newProgress, MaxProgress);
            if (!_isInitialized)
            {
                _isInitialized = true;
                InitializeSegmentReach();
                SnapProgressChange();
            }
            else
            {
                AnimateProgressChange();
                OnBarUpgradeStarted?.Invoke();
            }
        }

        private void InitializeFillParameters()
        {
            if (_fullWidth == 0)
            {
                _fullWidth = maskImage.rectTransform.sizeDelta.x;
            }
        }


        private void SnapProgressChange()
        {
            CalculateBarValues(out var barWidth, out var barAnchor);
            maskImage.rectTransform.sizeDelta = new Vector2(barWidth, maskImage.rectTransform.sizeDelta.y);
            maskImage.rectTransform.anchoredPosition = new Vector2(barAnchor, maskImage.rectTransform.anchoredPosition.y);
            maskImage.color = GetBarTargetColor();
            OnBarChanged?.Invoke();
        }

        private void AnimateProgressChange()
        {
            if (_fillAnchorTween != null || _fillWidthTween != null || _fillColorTween != null)
            {
                _fillAnchorTween.Kill();
                _fillWidthTween.Kill();
                _fillColorTween.Kill();
            }
            if (_segmentAnimationSequence != null)
            {
                _segments[Progress - 2].SetSegmentReach(true, true);
                _segmentAnimationSequence.Kill();
            }

            _segmentAnimationSequence = DOTween.Sequence();
            _segmentAnimationSequence.AppendInterval(fillDuration);
            _segmentAnimationSequence.AppendCallback(() => OnBarChanged?.Invoke());
            if (MaxProgress > Progress)
            {
                _segmentAnimationSequence.AppendCallback(() => { _segments[Progress - 1].SetSegmentReach(true, true); _segmentAnimationSequence = null; });
            }


            CalculateBarValues(out var barWidth, out var barAnchor);

            _fillWidthTween = maskImage.rectTransform.DOSizeDelta(new Vector2(barWidth, maskImage.rectTransform.sizeDelta.y), fillDuration);
            _fillAnchorTween = maskImage.rectTransform.DOAnchorPos(new Vector2(barAnchor, maskImage.rectTransform.anchoredPosition.y), fillDuration);
            _fillColorTween = maskImage.DOColor(GetBarTargetColor(), fillDuration);
        }


        private void InitializeSegmentReach()
        {
            if (_segments != null)
            {
                for (var i = 0; i < _segments.Count; i++)
                {
                    _segments[i].SetSegmentReach(i < Progress, false);
                }
            }
        }

        private void ResetSegments()
        {
            if (_segments != null)
            {
                for (var i = _segments.Count - 1; i >= 0; i--)
                {
                    Destroy(_segments[i].gameObject);
                }
            }
            _segments = new List<BarSegment>();

            for (var i = 1; i < MaxProgress; i++)
            {
                var newSegment = Instantiate(segmentPrefab, segmentContainer);

                _segments.Add(newSegment);
            }
        }

        private void CalculateBarValues(out float barWidth, out float barAnchor)
        {
            if (Progress == MaxProgress)
            {
                barWidth = _fullWidth;
            }
            else if (Progress == 0)
            {
                barWidth = 0;
            }
            else
            {
                barWidth = _segments[Progress - 1].GetSegmentPosition().x + fillTargetOffset;
            }

            barAnchor = barWidth / 2f + fillSourceOffset;
        }

        private Color GetBarTargetColor() => (Progress == MaxProgress ? fullColor : regularColor);
    }
}