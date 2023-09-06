using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SlimeBounce.Debugging.Modules
{
    public class DebugCounterFPS : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _view;
        [SerializeField] private float _updateCooldown;

        private float _nextUpdateTime;

        private void Update()
        {
            if (Time.unscaledTime > _nextUpdateTime)
            {
                _view.text = $"{(int)(1f / Time.unscaledDeltaTime)}";
                _nextUpdateTime = Time.unscaledTime + _updateCooldown;
            }
        }
    }
}