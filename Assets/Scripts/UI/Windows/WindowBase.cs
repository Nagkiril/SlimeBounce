using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeBounce.UI.Windows
{
    public class WindowBase : MonoBehaviour
    {
        [field: SerializeField] public WindowOpenType OpenType { get; private set; }
        [SerializeField] Animator ownAnimator;
        [SerializeField] AEContainer animEvents;
        [SerializeField] Button closeButton;

        public bool ControlAllowed
        {
            get => _isControlAllowed;
            set
            {
                _isControlAllowed = value;

            }
        }
        public bool IsShown { get; protected set; }
        protected bool _isControlAllowed;

        public event Action OnWindowClosed;

        protected virtual void Awake()
        {
            if (OpenType == WindowOpenType.Default)
            {
                Debug.LogWarning($"{gameObject} window has not been assigned a definitive WindowOpenType!");
            }
            if (closeButton != null)
                closeButton.onClick.AddListener(Close);
            animEvents.OnAnimationDone += NotifyWindowClosed;
        }

        protected virtual void Start()
        {
            if (!IsShown)
                SetHidden();
        }

        protected void ChangeShow(bool isShown)
        {
            IsShown = isShown;
            ownAnimator.SetBool("Shown", isShown);
        }

        protected void NotifyWindowClosed()
        {
            Destroy(gameObject);
            OnWindowClosed?.Invoke();
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
            ownAnimator.Play("Hidden");
            ownAnimator.Update(0f);
        }
    }

    public enum WindowOpenType
    {
        Default,
        MainWindow,
        SubWindow
    }
}