using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Debugging.Modules;
using SlimeBounce.Settings;

namespace SlimeBounce.Debugging
{
    public class DebugController : MonoBehaviour
    {
        //This feels very outdated, we can absolutely link prefabs and instances together and just work with them using same lines of code;
        //In my opinion it's not brutally horrible (I've seen much worse...), but there's a good notion of not reusing it, and rather rewriting it later in another project
        [SerializeField] private DebugConsole _consolePrefab;
        [SerializeField] private DebugPanel _panelPrefab;
        [SerializeField] private DebugCounterFPS _fpsPrefab;

        private DebugConsole _console;
        private DebugPanel _panel;
        private DebugCounterFPS _fps;


        // Start is called before the first frame update
        void Start()
        {
            if (DebugSettings.CheckDebugEnabled())
            {
                _fps = Instantiate(_fpsPrefab, transform);
                _panel = Instantiate(_panelPrefab, transform);
                _console = Instantiate(_consolePrefab, transform);
                _console.Hide();
                _panel.Toggle();
            } else
            {
                Destroy(gameObject);
            }
        }



    }
}