using System;
using System.Collections;
using System.Collections.Generic;
using SlimeBounce.Animations.Controllers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SlimeBounce.UI.Windows
{
    public class WindowBase : MonoBehaviour
    {
        [field: SerializeField] public WindowOpenType OpenType { get; private set; }
        [field: SerializeField] public bool PrepareAllowed { get; protected set; }
        [SerializeField] private ShowHideActiveController _showController;
        [SerializeField] private CustomButton _closeButton;
        protected bool _isControlAllowed;

        public bool ControlAllowed
        {
            get => _isControlAllowed;
            set
            {
                _isControlAllowed = value;

            }
        }
        public bool IsShown { get; protected set; }
        public event Action<WindowBase> OnWindowClosed;

        protected virtual void Awake()
        {
            if (OpenType == WindowOpenType.Default)
            {
                Debug.LogWarning($"{gameObject} window has not been assigned a definitive WindowOpenType!");
            }
            if (_closeButton != null)
                _closeButton.OnClicked += Close;
            _showController.OnAnimationFinish += OnShowFinished;
        }

        protected virtual void Start()
        {
            if (!IsShown)
                SetHidden();
        }

        protected void ChangeShow(bool isShown)
        {
            IsShown = isShown;
            if (isShown)
                _showController.Show();
            else
                _showController.Hide();
        }

        protected void NotifyWindowClosed()
        {
            OnWindowClosed?.Invoke(this);
        }

        protected virtual void OnShowFinished()
        {
            if (!_showController.IsShown)
                NotifyWindowClosed();
        }

        public virtual void Show() 
        {
            ChangeShow(true);
        }

        public virtual void Close()
        {
            ChangeShow(false);
        }

        public virtual void SetHidden()
        {
            IsShown = false;
            _showController.Hide(false);
        }

        //We're not doing Prepare() method as an interface (such as IWindowPreparable) in order to avoid having to cast windows multiple times (cast prefab to check interface 1st, then cast new instance to call the method on it)
        public virtual void Prepare()
        {

        }

        public class Factory : PrefabFactory<WindowBase>
        {

        }
    }

    public enum WindowOpenType
    {
        Default,
        MainWindow,
        SubWindow
    }
}