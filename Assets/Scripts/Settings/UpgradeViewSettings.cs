using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Settings.Generic;

namespace SlimeBounce.Settings
{
    [CreateAssetMenu(fileName = "UpgradeViewSettings", menuName = "SlimeBounce/New Upgrade View Settings", order = 10)]
    public class UpgradeViewSettings : GenericSettings<UpgradeViewSettings>
    {
        [SerializeField] UpgradeViewData[] viewData;

        Dictionary<UpgradeViewCategory, Dictionary<UpgradeType, UpgradeViewData>> _categoryCache;

        private const string _loadPath = "Settings/UpgradeViewSettings";
        private static UpgradeViewSettings instance => (UpgradeViewSettings)GetInstance(_loadPath);

        public static Dictionary<UpgradeType, UpgradeViewData> GetViewCategory(UpgradeViewCategory category)
        {
            return instance.GetCategory(category);
        }


        private Dictionary<UpgradeType, UpgradeViewData> GetCategory(UpgradeViewCategory category)
        {
            if (_categoryCache == null)
            {
                _categoryCache = new Dictionary<UpgradeViewCategory, Dictionary<UpgradeType, UpgradeViewData>>();
            }
            if (!_categoryCache.ContainsKey(category))
            {
                var categoryViewData = new Dictionary<UpgradeType, UpgradeViewData>();
                for (var i = 0; i < viewData.Length; i++)
                {
                    if (viewData[i].Category == category)
                    {
                        categoryViewData.Add((UpgradeType)i, viewData[i]);
                    }
                }
                _categoryCache.Add(category, categoryViewData);
            }
            return _categoryCache[category];
        }
    }

    [Serializable]
    public class UpgradeViewData
    {
        public UpgradeViewCategory Category;
        public string Name;
        public string Description;
        public Sprite Icon;

        public UpgradeViewData Clone()
        {
            var newData = new UpgradeViewData();
            newData.Category = Category;
            newData.Name = Name;
            newData.Description = Description;

            return newData;
        }
    }

    [Serializable]
    public enum UpgradeViewCategory
    {
        Passive,
        Ability
    }
}