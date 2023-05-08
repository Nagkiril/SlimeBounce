using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Debugging.Modules;
using SlimeBounce.Settings;

namespace SlimeBounce.Debugging
{
    public class DebugController : MonoBehaviour
    {
        [SerializeField] DebugConsole consolePrefab;
        [SerializeField] DebugPanel panelPrefab;
        [SerializeField] DebugCounterFPS fpsPrefab;

        DebugConsole _console;
        DebugPanel _panel;
        DebugCounterFPS _fps;


        // Start is called before the first frame update
        void Start()
        {
            if (DebugSettings.CheckDebugEnabled())
            {
                _fps = Instantiate(fpsPrefab, transform);
                _panel = Instantiate(panelPrefab, transform);
                _console = Instantiate(consolePrefab, transform);
                _console.Hide();
                _panel.Toggle();
            } else
            {
                Destroy(gameObject);
            }
        }



    }
}