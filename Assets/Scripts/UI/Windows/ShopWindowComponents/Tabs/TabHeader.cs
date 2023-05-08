using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeBounce.UI.Windows.ShopComponents.Tabs
{
    public class TabHeader : MonoBehaviour
    {
        [SerializeField] Button ownButton;
        [SerializeField] Animator ownAnim;

        public event Action OnHeaderClicked;

        private void Awake()
        {
            ownButton.onClick.AddListener(OnHeaderClick);
        }

        void OnHeaderClick()
        {
            OnHeaderClicked?.Invoke();
        }

        public void SetSelected(bool isSelected)
        {
            ownAnim.SetBool("SelectedTab", isSelected);
        }
    }
}