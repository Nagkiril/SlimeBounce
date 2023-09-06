using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeBounce.UI.Windows.ShopComponents.Tabs
{
    public class TabHeader : MonoBehaviour
    {
        [SerializeField] private CustomButton _ownButton;

        public event Action OnHeaderClicked;

        private void Awake()
        {
            _ownButton.OnClicked += OnHeaderClick;
        }

        private void OnHeaderClick()
        {
            OnHeaderClicked?.Invoke();
        }

        public void SetSelected(bool isSelected)
        {
            _ownButton.Interactable = !isSelected;
        }
    }
}