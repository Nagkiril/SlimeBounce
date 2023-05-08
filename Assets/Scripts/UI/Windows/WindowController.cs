using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Settings;

namespace SlimeBounce.UI.Windows
{
    public class WindowController : MonoBehaviour
    {
        static WindowController _instance;

        Stack<WindowBase> _openWindows;
        WindowBase _activeWindow;

        Queue<WindowBase> _queuedWindows;

        private void Awake()
        {
            if (_instance == null)
            {
                _openWindows = new Stack<WindowBase>();
                _queuedWindows = new Queue<WindowBase>();

                _instance = this;
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private T CreateNewWindow<T>(WindowOpenType openType)
        {
            var windowPrefab = WindowSettings.GetWindowPrefab<T>();
            if (windowPrefab != null)
            {
                var newWindow = Instantiate(windowPrefab, transform).GetComponent<WindowBase>();

                var appliedOpenType = (openType == WindowOpenType.Default? newWindow.OpenType : openType);

                if (appliedOpenType == WindowOpenType.MainWindow)
                    QueueWindow(newWindow);
                else
                    ActivateWindow(newWindow);

                return (T)(object)newWindow;
            }
            else
            {
                Debug.LogWarning($"Could not find a window prefab of type {typeof(T).Name}!");
                return default;
            }
        }

        void QueueWindow(WindowBase targetWindow)
        {
            _queuedWindows.Enqueue(targetWindow);
            targetWindow.SetHidden();
            if (_activeWindow == null || !_activeWindow.IsShown)
                ActivateNextWindow();
        }

        void ActivateWindow(WindowBase targetWindow)
        {
            _activeWindow = targetWindow;
            _activeWindow.OnWindowClosed += ActivateNextWindow;
            if (!_openWindows.Contains(targetWindow))
                _openWindows.Push(targetWindow);
            targetWindow.Show();
        }

        void ActivateNextWindow()
        {
            if (_openWindows.Count > 0)
                _openWindows.Pop();
            if (_openWindows.Count > 0)
            {
                ActivateWindow(_openWindows.Peek());
            } else
            {
                if (_queuedWindows.Count > 0)
                {
                    ActivateWindow(_queuedWindows.Dequeue());
                }
            }
        }

        public static T OpenWindow<T>(WindowOpenType openType = WindowOpenType.Default)
        {
            return _instance.CreateNewWindow<T>(openType);
        }
    }
}