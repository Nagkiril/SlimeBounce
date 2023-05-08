using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SlimeBounce.UI.Windows.ShopComponents.Tabs.Content
{
    public class PassiveCard : MonoBehaviour, ITabContent
    {
        [SerializeField] TextMeshProUGUI titleView;
        [SerializeField] TextMeshProUGUI descriptionView;
        [SerializeField] TextMeshProUGUI priceView;
        [SerializeField] Image iconView;
        [SerializeField] Button upgradeButton;
        [SerializeField] Animator ownAnim;
        [SerializeField] SegmentedBar bar;

        private bool _isInitialized;
        private string _descriptionTemplate;

        public event Action<PassiveCard> OnUpgradeCommand;

        void OnBarUpgradeStart()
        {
            ownAnim.SetTrigger("Upgrade");
        }

        void OnUpgradeClick()
        {
            OnUpgradeCommand?.Invoke(this);
        }

        public void SetCardView(string title, Sprite icon, string descriptionTemplate, int maxProgress)
        {
            titleView.text = title;
            iconView.sprite = icon;
            _descriptionTemplate = descriptionTemplate;
            bar.SetMaxProgress(maxProgress);
        }

        public void SetCardProgress(float currentValue, float upgradeDelta, int price, int progress)
        {
            descriptionView.text = string.Format(_descriptionTemplate, currentValue, upgradeDelta);
            priceView.text = price.ToString();
            bar.SetProgress(progress);
            CheckCardMaxUpgrade();
        }

        public void Initialize()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                bar.OnBarUpgradeStarted += OnBarUpgradeStart;
                upgradeButton.onClick.AddListener(OnUpgradeClick);
            }
        }


        public void ApplyContentCore(Transform core)
        {
            transform.SetParent(core, false);
        }

        void CheckCardMaxUpgrade()
        {
            if (bar.Progress == bar.MaxProgress)
            {
                upgradeButton.interactable = false;
                priceView.text = "MAX";
                ownAnim.SetBool("Maxed", true);
            }
        }
    }
}