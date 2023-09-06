using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.UI.Windows.ShopComponents.Tabs.Content
{
    public interface ITabContent
    {
        public void Initialize();

        public void ApplyContentCore(Transform core);
    }
}