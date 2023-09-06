using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.UI.Windows.Settings
{
    public class WindowSceneSettings : MonoBehaviour, IWindowSettings
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