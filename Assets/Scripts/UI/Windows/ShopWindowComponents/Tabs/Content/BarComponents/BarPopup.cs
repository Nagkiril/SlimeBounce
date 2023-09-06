using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Animations.Controllers;

namespace SlimeBounce.UI.Windows.ShopComponents.Tabs.Content.BarComponents
{
    public class BarPopup : MonoBehaviour, IBarAccessory
    {
        [SerializeField] private RectTransform _ownRect;
        [SerializeField] private ShowHideController _showController;
        [SerializeField] private SegmentedBar _targetBar;
        [SerializeField] private float _targetProgressRatio;

        private void Awake()
        {
            _targetBar.OnBarChanged += OnBarChange;
        }

        private void Start()
        {
            _targetBar.AttachAccessory(this, _targetProgressRatio);
        }

        private void OnDestroy()
        {
            _targetBar.OnBarChanged -= OnBarChange;
        }

        private void OnBarChange()
        {
            if (((float)_targetBar.Progress / _targetBar.MaxProgress) < _targetProgressRatio)
            {
                if (!_showController.IsShown)
                    _showController.Show();
            }
            else
            {
                if (_showController.IsShown)
                {
                    _showController.Hide();
                }
            }
        }

        public void AttachToTransform(RectTransform target)
        {
            Vector2 defaultAnchor = _ownRect.anchoredPosition;
            _ownRect.SetParent(target);
            _ownRect.anchoredPosition = defaultAnchor;
            _ownRect.localScale = Vector3.one;
        }
    }
}