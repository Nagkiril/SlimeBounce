using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SlimeBounce.UI.Windows.ShopComponents.Tabs.Content;

namespace SlimeBounce.UI.Windows.ShopComponents.Tabs
{
    public class TabContainer : MonoBehaviour
    {
        [SerializeField] Animator ownAnim;
        [SerializeField] AEContainer ownEvents;
        [SerializeField] Scrollbar ownScroll;
        [SerializeField] RectTransform contentCore;

        List<ITabContent> _tabContent;

        private void Start()
        {
            ownScroll.value = 1f;
            //We're giving ourselves time to let animation resolve, and when container is visible, we validate all content inside.
            ownEvents.OnActionA += OnVisibilityChange;
        }

        public void RegisterContent(ITabContent contentInstance)
        {
            contentInstance.ApplyContentCore(contentCore);
            if (_tabContent == null)
                _tabContent = new List<ITabContent>();
            _tabContent.Add(contentInstance);
        }

        public void Show()
        {
            ownAnim.SetBool("Show", true);
            transform.SetAsLastSibling();
        }

        void OnVisibilityChange()
        {
            if (_tabContent != null)
            {
                foreach (var content in _tabContent)
                {
                    content.Initialize();
                }
            }
            ownScroll.value = 1f;
        }

        public void Hide()
        {
            ownAnim.SetBool("Show", false);
        }
    }
}