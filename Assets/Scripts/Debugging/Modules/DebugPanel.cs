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
        [SerializeField] Button toggleButton;
        [SerializeField] DebugButtonContainer container;

        void Awake()
        {
            toggleButton.onClick.RemoveAllListeners();
            toggleButton.onClick.AddListener(Toggle);
            InitializeOptions();
        }

        public void Toggle()
        {
            var view = container.gameObject;
            view.SetActive(!view.activeSelf);
        }

        private void InitializeOptions()
        {
            container.MakeButton("Reset", DebugSaveReset);
        }

        private void DebugSaveReset()
        {
            SaveManager.ClearSaves();
            SceneLoader.ReloadScene();
        }
    }



}