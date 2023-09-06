using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Environment;
using SlimeBounce.Animations.Controllers;
using Zenject;


namespace SlimeBounce.UI
{
    public class LevelSwitchUI : MonoBehaviour
    {
        [SerializeField] private bool _visibleOnGameplay;
        [SerializeField] private bool _visibleOnLobby;
        [SerializeField] private bool _visibleOnLevelResolution;
        [SerializeField] private ShowHideController _showController;

        [Inject]
        private ILevelStateProvider _levelState;

        private void Start()
        {
            _levelState.OnLevelStarted += OnLevelStart;
            _levelState.OnLevelEnded += OnLevelEnd;
            _levelState.OnLobbyEntered += OnLobbyEnter;
            OnLobbyEnter();
        }

        private void OnDestroy()
        {
            _levelState.OnLevelStarted -= OnLevelStart;
            _levelState.OnLevelEnded -= OnLevelEnd;
            _levelState.OnLobbyEntered -= OnLobbyEnter;
        }

        private void OnLevelStart()
        {
            SetVisibility(_visibleOnGameplay);
        }

        private void OnLevelEnd(bool isWin)
        {
            SetVisibility(_visibleOnLevelResolution);
        }

        private void OnLobbyEnter()
        {
            SetVisibility(_visibleOnLevelResolution);
        }

        private void SetVisibility(bool isVisible)
        {
            if (isVisible)
            {
                if (!_showController.IsShown)
                    _showController.Show();
            }
            else
            {
                if (_showController.IsShown)
                    _showController.Hide();
            }
        }
    }
}