using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SlimeBounce.Debugging.Modules.PanelSubmodules;
using SlimeBounce.Save;
using SlimeBounce.Loading;
using SlimeBounce.Player;

namespace SlimeBounce.Debugging.Modules
{
    public class DebugPanel : MonoBehaviour
    {
        [SerializeField] private Button _toggleButton;
        [SerializeField] private DebugButtonContainer _container;

        private void Awake()
        {
            _toggleButton.onClick.RemoveAllListeners();
            _toggleButton.onClick.AddListener(Toggle);
            InitializeOptions();
        }

        private void InitializeOptions()
        {
            _container.MakeButton("Reset", DebugSaveReset);
        }

        private void DebugSaveReset()
        {
            SaveManager.ClearSaves();
            SceneLoader.ReloadScene();
        }

        public void Toggle()
        {
            var view = _container.gameObject;
            view.SetActive(!view.activeSelf);
        }
    }



}