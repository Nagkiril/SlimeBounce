using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Settings.Generic;
using SlimeBounce.Slime;

namespace SlimeBounce.Settings
{
    [CreateAssetMenu(fileName = "DebugSettings", menuName = "SlimeBounce/New Debug Settings", order = 10)]
    public class DebugSettings : GenericSettings<DebugSettings>
    {
        [SerializeField] protected bool enableDebugTools;

        private const string _loadPath = "Settings/DebugSettings";
        private static DebugSettings instance => (DebugSettings)GetInstance(_loadPath);

        public static bool CheckDebugEnabled() => instance.enableDebugTools;
    }
}