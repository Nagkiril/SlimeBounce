using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SlimeBounce.UI.Windows.ShopComponents;


namespace SlimeBounce.UI.Windows
{
    public class WindowShop : WindowBase
    {
        [SerializeField] ContentTab[] tabs;


        protected override void Start()
        {
            base.Start();
            foreach (var tab in tabs)
            {
                tab.OnHeaderClicked += OnTabClick;
            }
            ActivateTab(tabs[0]);
        }


        private void OnTabClick(ContentTab selectedTab)
        {
            ActivateTab(selectedTab);
        }

        void ActivateTab(ContentTab newActiveTab)
        {
            foreach (var tab in tabs)
            {
                if (tab != newActiveTab)
                {
                    tab.DeactivateTab();
                }
            }
            newActiveTab.ActivateTab();
        }

    }
}