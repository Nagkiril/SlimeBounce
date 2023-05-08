using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Settings.Generic;

namespace SlimeBounce.Settings
{
    [CreateAssetMenu(fileName = "DropoutSettings", menuName = "SlimeBounce/New Dropout Settings", order = 10)]
    public class DropoutSettings : GenericSettings<DropoutSettings>
    {
        [SerializeField] float dropoutCooldown;
        [SerializeField] int dropoutAmount;

        private const string _loadPath = "Settings/DropoutSettings";
        private static DropoutSettings instance => (DropoutSettings)GetInstance(_loadPath);

        public static float GetDropoutBaseCooldown()
        {
            return instance.dropoutCooldown;
        }

        public static int GetDropoutBaseAmount()
        {
            return instance.dropoutAmount;
        }
    }
}