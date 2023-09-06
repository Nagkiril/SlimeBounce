using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Player.Settings;
using SlimeBounce.Settings.Generic;
using SlimeBounce.UI.Settings;

namespace SlimeBounce.Settings
{
    [CreateAssetMenu(fileName = "UpgradeViewSettings", menuName = "SlimeBounce/New Upgrade View Settings", order = 10)]
    public class UpgradeViewSettings : ScriptableObject, IUpgradeViewSettings
    {
        [SerializeField] private UpgradeViewData[] _viewData;

        private Dictionary<UpgradeViewCategory, Dictionary<UpgradeType, UpgradeViewData>> _categoryCache;

        private Dictionary<UpgradeType, UpgradeViewData> GetCategory(UpgradeViewCategory category)
        {
            if (_categoryCache == null)
            {
                _categoryCache = new Dictionary<UpgradeViewCategory, Dictionary<UpgradeType, UpgradeViewData>>();
            }
            if (!_categoryCache.ContainsKey(category))
            {
                var categoryViewData = new Dictionary<UpgradeType, UpgradeViewData>();
                for (var i = 0; i < _viewData.Length; i++)
                {
                    if (_viewData[i].Category == category)
                    {
                        categoryViewData.Add((UpgradeType)i, _viewData[i]);
                    }
                }
                _categoryCache.Add(category, categoryViewData);
            }
            return _categoryCache[category];
        }

        public Dictionary<UpgradeType, UpgradeViewData> GetViewCategory(UpgradeViewCategory category)
        {
            return GetCategory(category);
        }

        public UpgradeViewData GetViewForType(UpgradeType type)
        {
            return _viewData[(int)type];
        }
    }
}