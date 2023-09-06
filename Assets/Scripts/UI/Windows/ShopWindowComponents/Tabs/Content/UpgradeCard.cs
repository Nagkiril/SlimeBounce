using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SlimeBounce.UI.Windows.ShopComponents.Tabs.Content
{
    public class UpgradeCard : MonoBehaviour, ITabContent
    {
        [SerializeField] private TextMeshProUGUI _titleView;
        [SerializeField] private TextMeshProUGUI _descriptionView;
        [SerializeField] private TextMeshProUGUI _priceView;
        [SerializeField] private Image _iconView;
        [SerializeField] private CustomButton _upgradeButton;
        [SerializeField] private SegmentedBar _bar;
        private bool _isInitialized;
        private string _descriptionTemplate;

        public event Action<UpgradeCard> OnUpgradeCommand;

        private void OnBarUpgradeStart()
        {

        }

        private void OnUpgradeClick()
        {
            OnUpgradeCommand?.Invoke(this);
        }

        private void CheckCardMaxUpgrade()
        {
            if (_bar.Progress == _bar.MaxProgress)
            {
                _upgradeButton.Interactable = false;
                _priceView.text = "MAX";
            }
        }

        public void SetCardView(string title, Sprite icon, string descriptionTemplate, int maxProgress)
        {
            _titleView.text = title;
            _iconView.sprite = icon;
            _descriptionTemplate = descriptionTemplate;
            _bar.SetMaxProgress(maxProgress);
        }

        public void SetCardProgress(List<float> descriptionValues, int price, int progress)
        {
            var descriptionArguments = new List<string>();
            foreach (var value in descriptionValues)
            {
                descriptionArguments.Add(value.ToString());
            }
            _descriptionView.text = string.Format(_descriptionTemplate, descriptionArguments.ToArray());
            _priceView.text = price.ToString();
            _bar.SetProgress(progress);
            CheckCardMaxUpgrade();
        }

        public void Initialize()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                _bar.OnBarUpgradeStarted += OnBarUpgradeStart;
                _upgradeButton.OnClicked += OnUpgradeClick;
            }
        }

        public void ApplyContentCore(Transform core)
        {
            transform.SetParent(core, false);
        }
    }
}