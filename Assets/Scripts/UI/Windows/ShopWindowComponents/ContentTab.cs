using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.UI.Windows.ShopComponents.Tabs;

namespace SlimeBounce.UI.Windows.ShopComponents
{
    public abstract class ContentTab : MonoBehaviour
    {
        [SerializeField] protected TabHeader _header;
        [SerializeField] protected TabContainer _container;
        public event Action<ContentTab> OnHeaderClicked;
        
        protected virtual void Awake()
        {
            _header.OnHeaderClicked += OnHeaderClick;
        }

        protected virtual void Start()
        {

        }

        protected virtual void OnHeaderClick()
        {
            OnHeaderClicked?.Invoke(this);
        }

        protected virtual void HideTab()
        {
            _header.SetSelected(false);
            _container.Hide();
        }

        protected virtual void ShowTab()
        {
            _header.SetSelected(true);
            _container.Show();
        }

        public virtual void ActivateTab()
        {
            ShowTab();
        }

        public virtual void DeactivateTab()
        {
            HideTab();
        }

        public virtual void Prepare()
        {

        }
    }
}