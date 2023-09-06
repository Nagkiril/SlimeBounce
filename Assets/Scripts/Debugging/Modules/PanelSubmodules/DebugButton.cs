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
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private Image _background;
        [SerializeField] private Button _button;

        public void SetTitle(string newTitle)
        {
            _title.text = newTitle;
        }

        public void SetColor(Color newColor)
        {
            _background.color = newColor;
        }

        public void SetClickAction(Action clickAction)
        {
            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(() => { clickAction?.Invoke(); });
        }
    }
}