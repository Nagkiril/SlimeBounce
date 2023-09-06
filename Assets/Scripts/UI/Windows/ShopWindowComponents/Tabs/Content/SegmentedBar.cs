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
        [SerializeField] private BarSegment _segmentPrefab;
        [SerializeField] private Transform _segmentContainer;
        [SerializeField] private Image _maskImage;
        [SerializeField] private Color _regularColor;
        [SerializeField] private Color _fullColor;
        [SerializeField] private float _fillTargetOffset;
        [SerializeField] private float _fillSourceOffset;
        [SerializeField] private float _fillDuration;
        [SerializeField] private RectTransform _barBeginning;
        [SerializeField] private RectTransform _barEnd;

        public event Action OnBarUpgradeStarted;
        public event Action OnBarChanged;

        //This looks very much like an interface, I think it should be separated\made into a separate component?
        public int MaxProgress { get; private set; }
        public int Progress { get; private set; }

        private bool _isInitialized;
        private List<BarSegment> _segments;
        private Tween _fillWidthTween;
        private Tween _fillAnchorTween;
        private Tween _fillColorTween;
        private Sequence _segmentAnimationSequence;
        private float _fullWidth;

        private void InitializeFillParameters()
        {
            if (_fullWidth == 0)
            {
                _fullWidth = _maskImage.rectTransform.sizeDelta.x;
            }
        }

        private void SnapProgressChange()
        {
            CalculateBarValues(out var barWidth, out var barAnchor);
            _maskImage.rectTransform.sizeDelta = new Vector2(barWidth, _maskImage.rectTransform.sizeDelta.y);
            _maskImage.rectTransform.anchoredPosition = new Vector2(barAnchor, _maskImage.rectTransform.anchoredPosition.y);
            _maskImage.color = GetBarTargetColor();
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
            _segmentAnimationSequence.AppendInterval(_fillDuration);
            _segmentAnimationSequence.AppendCallback(() => OnBarChanged?.Invoke());
            if (MaxProgress > Progress)
            {
                _segmentAnimationSequence.AppendCallback(() => { _segments[Progress - 1].SetSegmentReach(true, true); _segmentAnimationSequence = null; });
            }


            CalculateBarValues(out var barWidth, out var barAnchor);

            _fillWidthTween = _maskImage.rectTransform.DOSizeDelta(new Vector2(barWidth, _maskImage.rectTransform.sizeDelta.y), _fillDuration);
            _fillAnchorTween = _maskImage.rectTransform.DOAnchorPos(new Vector2(barAnchor, _maskImage.rectTransform.anchoredPosition.y), _fillDuration);
            _fillColorTween = _maskImage.DOColor(GetBarTargetColor(), _fillDuration);
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
                var newSegment = Instantiate(_segmentPrefab, _segmentContainer);

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
                if (_isInitialized)
                    barWidth = _segments[Progress - 1].GetSegmentPosition().x + _fillTargetOffset;
                else
                {
                    var fullActiveWidth = _fullWidth - _fillTargetOffset + _fillSourceOffset;
                    //Here we account according to the rough estimation of layout rules, with adjustments towards the mask's position
                    //Actual segment position is yet unavailable
                    var regularSegmentSize = fullActiveWidth / (MaxProgress - 1);
                    var edgeSegmentSize = regularSegmentSize / 2f;
                    //Notice how, despite no mod operations, math will align with both odd and even amount of segments
                    barWidth = edgeSegmentSize + regularSegmentSize * (Progress - 1) + _fillTargetOffset;
                }
            }

            barAnchor = barWidth / 2f + _fillSourceOffset;
        }

        private Color GetBarTargetColor() => (Progress == MaxProgress ? _fullColor : _regularColor);

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
                InitializeSegmentReach();
                SnapProgressChange();
                _isInitialized = true;
            }
            else
            {
                AnimateProgressChange();
                OnBarUpgradeStarted?.Invoke();
            }
        }

        public void AttachAccessory(IBarAccessory accessory, float segmentProgressRatio = -1f)
        {
            if (segmentProgressRatio >= 0 && segmentProgressRatio <= 1f)
            {
                var segmentIndex = Mathf.FloorToInt(MaxProgress * segmentProgressRatio) - 1;
                RectTransform targetTransform;
                if (segmentIndex <= -1)
                {
                    targetTransform = _barBeginning;
                } else if (segmentIndex >= _segments.Count)
                {
                    targetTransform = _barEnd;
                } else
                {
                    targetTransform = _segments[segmentIndex].GetAttachmentRect();
                }
                accessory.AttachToTransform(targetTransform);
            }
        }
    }
}