using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SlimeBounce.Debugging.Modules
{
    public class DebugCounterFPS : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI view;
        [SerializeField] float updateCooldown;

        float _nextUpdateTime;

        // Update is called once per frame
        void Update()
        {
            if (Time.unscaledTime > _nextUpdateTime)
            {
                view.text = $"{(int)(1f / Time.unscaledDeltaTime)}";
                _nextUpdateTime = Time.unscaledTime + updateCooldown;
            }
        }
    }
}