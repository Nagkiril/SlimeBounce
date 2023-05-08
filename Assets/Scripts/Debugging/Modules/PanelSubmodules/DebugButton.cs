using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SlimeBounce.Debugging.Modules.PanelSubmodules
{
    public class DebugButton : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] Image background;
        [SerializeField] Button button;

        public void SetTitle(string newTitle)
        {
            title.text = newTitle;
        }

        public void SetColor(Color newColor)
        {
            background.color = newColor;
        }

        public void SetClickAction(Action clickAction)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => { clickAction?.Invoke(); });
        }
    }
}