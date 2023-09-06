using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Player.Settings;

namespace SlimeBounce.UI.Settings
{
    public interface IUpgradeViewSettings
    {
        public Dictionary<UpgradeType, UpgradeViewData> GetViewCategory(UpgradeViewCategory category);

        public UpgradeViewData GetViewForType(UpgradeType type);
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