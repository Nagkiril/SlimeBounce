using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Settings.Generic;
using SlimeBounce.Slime;

namespace SlimeBounce.Settings
{
    //I'm leaving Debug Settings without DI, because I'd prefer not to couple debug tools with Zenject
    [CreateAssetMenu(fileName = "DebugSettings", menuName = "SlimeBounce/New Debug Settings", order = 10)]
    public class DebugSettings : GenericSettings<DebugSettings>
    {
        [SerializeField] protected bool _enableDebugTools;

        private const string _loadPath = "Settings/DebugSettings";
        private static DebugSettings _instance => (DebugSettings)GetInstance(_loadPath);

        public static bool CheckDebugEnabled() => _instance._enableDebugTools;
    }
}