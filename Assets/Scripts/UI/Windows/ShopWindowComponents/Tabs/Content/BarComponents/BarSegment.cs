using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SlimeBounce.Animations.Controllers;

namespace SlimeBounce.UI.Windows.ShopComponents.Tabs.Content.BarComponents
{
    public class BarSegment : MonoBehaviour
    {
        [SerializeField] private RectTransform _ownRect;
        [SerializeField] private Image _segmentVisual;
        [SerializeField] private ShowHideController _showController;

        public void SetSegmentReach(bool isReached, bool animate)
        {
            if (isReached)
                _showController.Show(animate);
            else
                _showController.Hide(animate);
        }

        public RectTransform GetAttachmentRect() => _ownRect;

        public Vector2 GetSegmentPosition() => _ownRect.anchoredPosition;
    }
}