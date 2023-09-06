using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SlimeBounce.Debugging.Modules
{
    public class DebugConsole : MonoBehaviour
    {
        [SerializeField] private GameObject _viewCore;
        [SerializeField] private TextMeshProUGUI _logContainer;

        private Queue<LogEntry> _logsToProcess;
        private float _toggleCooldown;

        const float TOGGLE_SQR_MAGNITUDE = 4f;
        const float TOGGLE_COOLDOWN = 1.5f;


        protected struct LogEntry
        {
            public string Message;
            public string Stack;
            public LogType Type;
        }

        private void Awake()
        {
            _logsToProcess = new Queue<LogEntry>();
            Application.logMessageReceived += OnMessageLogged;
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= OnMessageLogged;
        }

        private void Update()
        {
            ProcessLogQueue();
            ProcessConsoleToggle();
        }

        private void ProcessConsoleToggle()
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

        private void ProcessLogQueue()
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

                    _logContainer.text += nextMessage + "\n";
                }
            }
        }

        private void OnMessageLogged(string condition, string stackTrace, LogType type)
        {
            _logsToProcess.Enqueue(new LogEntry { Message = condition, Stack = stackTrace, Type = type});
        }

        public void Hide()
        {
            _viewCore.SetActive(false);
        }

        public void Show()
        {
            _viewCore.SetActive(true);
        }

        public bool IsVisible() => _viewCore.activeSelf;
    }
}