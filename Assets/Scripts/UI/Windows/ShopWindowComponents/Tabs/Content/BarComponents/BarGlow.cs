using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace SlimeBounce.UI.Windows.ShopComponents.Tabs.Content.BarComponents
{
    public class BarGlow : MonoBehaviour
    {
        [SerializeField] private SegmentedBar _targetBar;
        [SerializeField] private Image _glowImage;
        [SerializeField] private Color _glowColor;
        [SerializeField] private float _glowDuration;

        private Sequence _colorSequence;
        private bool _isActive;
        private bool _isInitialized;

        private void Awake()
        {
            _targetBar.OnBarChanged += CheckGlowActivity;
        }

        private void OnDestroy()
        {
            _targetBar.OnBarChanged -= CheckGlowActivity;
        }

        private void CheckGlowActivity()
        {
            bool shouldBeActive = CheckTargetActivity();
            if (_isActive != shouldBeActive || !_isInitialized)
            {
                _isInitialized = true;
                _isActive = shouldBeActive;
                if (_colorSequence != null)
                {
                    _colorSequence.Kill();
                }
                _colorSequence = DOTween.Sequence();
                _colorSequence.Append(_glowImage.DOColor(GetTargetColor(), _glowDuration));
            }
        }

        private bool CheckTargetActivity()
        {
            //Here we might implement any additional activation parameters, if we'll need any
            return _targetBar.Progress == _targetBar.MaxProgress;
        }

        private Color GetTargetColor()
        {
            return (_isActive ? _glowColor : Color.clear); 
        }
    }
}
