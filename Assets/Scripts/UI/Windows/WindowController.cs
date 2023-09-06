using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SlimeBounce.UI.Windows.Settings;
using Zenject;

namespace SlimeBounce.UI.Windows
{
    public class WindowController : MonoBehaviour, IWindowActor
    {
        [SerializeField] private Image _backgroundImage;
        private WindowBase _activeWindow;
        private Stack<WindowBase> _openWindows;
        private Queue<WindowBase> _queuedWindows;

        [Inject]
        private IWindowSettings _windowSettings;
        [Inject]
        private IWindowInstanceProvider _windowProvider;

        private void Awake()
        {
            _openWindows = new Stack<WindowBase>();
            _queuedWindows = new Queue<WindowBase>();
            CheckBackgroundActivity();
        }

        private T CreateNewWindow<T>(WindowOpenType openType) where T : WindowBase
        {
            var windowPrefab = _windowSettings.GetWindowPrefab<T>();
            if (windowPrefab != null)
            {
                var newWindow = _windowProvider.GetWindowInstance<T>(transform);
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

        private void QueueWindow(WindowBase targetWindow)
        {
            _queuedWindows.Enqueue(targetWindow);
            targetWindow.SetHidden();
            if (!HasActiveWindow())
                ActivateNextWindow();
        }

        private void ActivateWindow(WindowBase targetWindow)
        {
            _activeWindow = targetWindow;
            _activeWindow.OnWindowClosed += OnWindowClosed;
            if (!_openWindows.Contains(targetWindow))
                _openWindows.Push(targetWindow);
            targetWindow.Show();
        }

        private void OnWindowClosed(WindowBase targetWindow)
        {
            targetWindow.OnWindowClosed -= OnWindowClosed;
            ActivateNextWindow();
        }

        private void ActivateNextWindow()
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
            CheckBackgroundActivity();
        }

        private void CheckBackgroundActivity()
        {
            _backgroundImage.gameObject.SetActive(HasActiveWindow());
        }

        public bool HasActiveWindow()
        {
            return _activeWindow != null && _activeWindow.IsShown;
        }

        public T OpenWindow<T>(WindowOpenType openType = WindowOpenType.Default) where T : WindowBase
        {
            return CreateNewWindow<T>(openType);
        }
    }
}