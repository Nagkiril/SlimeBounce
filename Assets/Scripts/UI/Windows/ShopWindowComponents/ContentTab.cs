using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.UI.Windows.ShopComponents.Tabs;

namespace SlimeBounce.UI.Windows.ShopComponents
{
    public abstract class ContentTab : MonoBehaviour
    {
        [SerializeField] protected TabHeader header;
        [SerializeField] protected TabContainer container;
        public event Action<ContentTab> OnHeaderClicked;
        
        protected virtual void Awake()
        {
            header.OnHeaderClicked += OnHeaderClick;
        }

        protected virtual void Start()
        {

        }

        public virtual void ActivateTab()
        {
            ShowTab();
        }

        public virtual void DeactivateTab()
        {
            HideTab();
        }

        protected virtual void OnHeaderClick()
        {
            OnHeaderClicked?.Invoke(this);
        }

        protected virtual void HideTab()
        {
            header.SetSelected(false);
            container.Hide();
        }

        protected virtual void ShowTab()
        {
            header.SetSelected(true);
            container.Show();
        }
    }
}