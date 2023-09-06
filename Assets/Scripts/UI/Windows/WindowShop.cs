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
        [SerializeField] private ContentTab[] _tabs;


        protected override void Start()
        {
            base.Start();
            foreach (var tab in _tabs)
            {
                tab.OnHeaderClicked += OnTabClick;
            }
        }


        private void OnTabClick(ContentTab selectedTab)
        {
            ActivateTab(selectedTab);
        }

        private void ActivateTab(ContentTab newActiveTab)
        {
            foreach (var tab in _tabs)
            {
                if (tab != newActiveTab)
                {
                    tab.DeactivateTab();
                }
            }
            newActiveTab.ActivateTab();
        }

        public override void Show()
        {
            base.Show();
            ActivateTab(_tabs[0]);
        }

        public override void Prepare()
        {
            foreach (var tab in _tabs)
            {
                tab.Prepare();
            }
        }

    }
}