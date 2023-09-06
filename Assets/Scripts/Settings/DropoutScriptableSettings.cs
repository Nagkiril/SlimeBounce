using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Environment.Settings;

namespace SlimeBounce.Settings
{
    [CreateAssetMenu(fileName = "DropoutSettings", menuName = "SlimeBounce/New Dropout Settings", order = 10)]
    public class DropoutScriptableSettings : ScriptableObject, IDropoutSettings
    {
        [SerializeField] private float _dropoutCooldown;
        [SerializeField] private int _dropoutAmount;

        public float DropoutBaseCooldown => _dropoutCooldown;

        public int DropoutBaseAmount => _dropoutAmount;

    }
}