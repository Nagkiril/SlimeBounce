using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.UI.Windows.Settings
{
    public interface IWindowSettings
    {
        public T GetWindowPrefab<T>() where T : WindowBase;

        public IEnumerable<WindowBase> GetAllWindowPrefabs();
    }
}