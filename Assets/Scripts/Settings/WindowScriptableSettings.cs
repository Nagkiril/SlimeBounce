using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Settings.Generic;
using SlimeBounce.UI.Windows;
using SlimeBounce.UI.Windows.Settings;

namespace SlimeBounce.Settings
{
    [CreateAssetMenu(fileName = "WindowSettings", menuName = "SlimeBounce/New Window Settings", order = 10)]
    public class WindowScriptableSettings : ScriptableObject, IWindowSettings
    {
        [SerializeField] private WindowBase[] _windowPrefabs;

        public T GetWindowPrefab<T>() where T : WindowBase
        {
            foreach (var window in _windowPrefabs)
            {
                if (window is T)
                    return (T)window;
            }
            return null;
        }

        public IEnumerable<WindowBase> GetAllWindowPrefabs() => _windowPrefabs;
    }
}