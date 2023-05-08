using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Environment;

namespace SlimeBounce.UI
{
    public class LevelSwitchUI : MonoBehaviour
    {
        [SerializeField] bool visibleOnGameplay;
        [SerializeField] bool visibleOnLobby;
        [SerializeField] bool visibleOnLevelResolution;
        [SerializeField] Animator ownAnimator;

        // Start is called before the first frame update
        void Start()
        {
            LevelController.OnLevelStarted += OnLevelStart;
            LevelController.OnLevelEnded += OnLevelEnd;
            LevelController.OnLobbyEntered += OnLobbyEnter;
        }

        private void OnDestroy()
        {
            LevelController.OnLevelStarted -= OnLevelStart;
            LevelController.OnLevelEnded -= OnLevelEnd;
            LevelController.OnLobbyEntered -= OnLobbyEnter;
        }

        private void OnLevelStart()
        {
            SetVisibility(visibleOnGameplay);
        }

        private void OnLevelEnd(bool isWin)
        {
            SetVisibility(visibleOnLevelResolution);
        }

        private void OnLobbyEnter()
        {
            SetVisibility(visibleOnLevelResolution);
        }

        private void SetVisibility(bool isVisible)
        {
            ownAnimator.SetBool("Shown", isVisible);
        }
    }
}