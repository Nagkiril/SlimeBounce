using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SlimeBounce.Debugging.Modules
{
    public class DebugConsole : MonoBehaviour
    {
        [SerializeField] GameObject viewCore;
        [SerializeField] TextMeshProUGUI logContainer;

        Queue<LogEntry> _logsToProcess;

        const float TOGGLE_SQR_MAGNITUDE = 4f;
        const float TOGGLE_COOLDOWN = 1.5f;
        float _toggleCooldown;

        protected struct LogEntry
        {
            public string Message;
            public string Stack;
            public LogType Type;
        }


        // Start is called before the first frame update
        void Awake()
        {
            _logsToProcess = new Queue<LogEntry>();
            Application.logMessageReceived += OnMessageLogged;
        }


        // Update is called once per frame
        void Update()
        {
            ProcessLogQueue();
            ProcessConsoleToggle();
        }

        void ProcessConsoleToggle()
        {
            if (_toggleCooldown > 0f)
                _toggleCooldown -= Time.deltaTime;
            if (Input.acceleration.sqrMagnitude > TOGGLE_SQR_MAGNITUDE && _toggleCooldown <= 0f)
            {
                _toggleCooldown = TOGGLE_COOLDOWN;
                if (IsVisible())
                    Hide();
                else
                    Show();
            }
        }

        void ProcessLogQueue()
        {
            if (_logsToProcess.Count > 0)
            {
                while (_logsToProcess.Count > 0)
                {
                    var nextLog = _logsToProcess.Dequeue();
                    var nextMessage = nextLog.Message;

                    switch (nextLog.Type)
                    {
                        case LogType.Error:
                            nextMessage = $"<color=red>" + nextMessage + $"</color>";
                            break;
                        case LogType.Warning:
                            nextMessage = $"<color=yellow>" + nextMessage + $"</color>";
                            break;
                    }

                    logContainer.text += nextMessage + "\n";
                }
            }
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= OnMessageLogged;
        }

        private void OnMessageLogged(string condition, string stackTrace, LogType type)
        {
            _logsToProcess.Enqueue(new LogEntry { Message = condition, Stack = stackTrace, Type = type});
        }

        public void Hide()
        {
            viewCore.SetActive(false);
        }

        public void Show()
        {
            viewCore.SetActive(true);
        }

        public bool IsVisible() => viewCore.activeSelf;
    }
}