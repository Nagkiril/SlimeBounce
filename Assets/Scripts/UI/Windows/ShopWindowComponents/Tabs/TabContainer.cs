using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SlimeBounce.Animations.Controllers;
using SlimeBounce.UI.Windows.ShopComponents.Tabs.Content;

namespace SlimeBounce.UI.Windows.ShopComponents.Tabs
{
    public class TabContainer : MonoBehaviour
    {
        [SerializeField] private ShowHideController _showController;
        [SerializeField] private Scrollbar _ownScroll;
        [SerializeField] private RectTransform _contentCore;
        private List<ITabContent> _tabContent;

        private void Start()
        {
            _ownScroll.value = 1f;
            //We're giving ourselves time to let animation resolve, and when container is visible, we validate all content inside.
            _showController.OnAnimationFinish += OnVisibilityChange;
        }

        private void OnVisibilityChange()
        {
            if (_tabContent != null)
            {
                foreach (var content in _tabContent)
                {
                    content.Initialize();
                }
            }
            _ownScroll.value = 1f;
        }

        public void RegisterContent(ITabContent contentInstance)
        {
            contentInstance.ApplyContentCore(_contentCore);
            if (_tabContent == null)
                _tabContent = new List<ITabContent>();
            _tabContent.Add(contentInstance);
        }

        public void Show()
        {
            _showController.Show();
            transform.SetAsLastSibling();
        }

        public void Hide()
        {
            _showController.Hide();
        }
    }
}