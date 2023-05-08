using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Settings.Generic;
using SlimeBounce.Slime;
using SlimeBounce.UI.Windows;

namespace SlimeBounce.Settings
{
    [CreateAssetMenu(fileName = "WindowSettings", menuName = "SlimeBounce/New Window Settings", order = 10)]
    public class WindowSettings : GenericSettings<WindowSettings>
    {
        [SerializeField] WindowBase[] windowPrefabs;

        private const string _loadPath = "Settings/WindowSettings";
        private static WindowSettings instance => (WindowSettings)GetInstance(_loadPath);

        public static GameObject GetWindowPrefab<T>()
        {
            foreach (var window in instance.windowPrefabs)
            {
                if (window is T)
                    return window.gameObject;
            }
            return null;
        }
    }
}