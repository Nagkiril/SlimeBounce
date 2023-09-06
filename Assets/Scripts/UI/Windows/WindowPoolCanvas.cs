using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.UI.Windows.Settings;
using Zenject;

namespace SlimeBounce.UI.Windows
{
    //This is something like a pool for windows, except we're preparing some specific window types in advance based on their settings
    public class WindowPoolCanvas : MonoBehaviour, IWindowInstanceProvider
    {
        private List<WindowBase> _preparedWindows;

        [Inject]
        private IWindowSettings _windowSettings;
        [Inject]
        private WindowBase.Factory _windowFactory;


        private void Awake()
        {
            _preparedWindows = new List<WindowBase>();
            foreach (var windowPrefab in _windowSettings.GetAllWindowPrefabs())
            {
                if (windowPrefab.PrepareAllowed)
                {
                    var newInstance = _windowFactory.Create(windowPrefab);
                    newInstance.transform.SetParent(transform, false);
                    newInstance.Prepare();
                    //Notice how on specifically first creation we're NOT turning off the component; that is so other components inside can initialize properly and opening the window will be as smooth as possible
                    //However, this is avoidable if we're not using anything heavy in the window - which we shouldn't do in UI anyway - but for now I'm leaving it on
                    //newInstance.gameObject.SetActive(false);
                    _preparedWindows.Add(newInstance);
                }
            }
        }

        private void OnWindowClosed(WindowBase window)
        {
            if (window.PrepareAllowed)
            {
                window.gameObject.SetActive(false);
                window.transform.SetParent(transform);
                _preparedWindows.Add(window);
            }
            else
            {
                //We're not pooling non-preparable windows, because they are not accounting for reuse; this can be changed
                Destroy(window.gameObject);
            }
        }


        private T GetPreparedWindow<T>() where T : WindowBase
        {
            foreach (var window in _preparedWindows)
            {
                if (window is T targetWindow)
                {
                    window.gameObject.SetActive(true);
                    return targetWindow;
                }
            }
            return default(T);
        }

        public T GetWindowInstance<T>(Transform newInstanceRoot) where T : WindowBase
        {
            var newInstance = GetPreparedWindow<T>();
            if (newInstance == null)
            {
                newInstance = (T)_windowFactory.Create(_windowSettings.GetWindowPrefab<T>());
                newInstance.transform.SetParent(transform, false);
                newInstance.Prepare();
            }
            //I'm thinking that for now we shouldn't reparent the window as it is a bit of a performance hit with no tangible upsides
            //newInstance.transform.SetParent(newInstanceRoot, false);
            newInstance.OnWindowClosed += OnWindowClosed;
            return newInstance;
        }
    }
}