using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.Debugging.Modules.PanelSubmodules
{
    public class DebugButtonContainer : MonoBehaviour
    {
        [SerializeField] DebugButton simpleButton;


        public DebugButton MakeButton(string title, Action onClick)
        {
            var newButton = Instantiate(simpleButton, transform);
            newButton.SetClickAction(onClick);
            newButton.SetTitle(title);
            return newButton;
        }

        public DebugButton MakeButton(string title, Color color, Action onClick)
        {
            var newButton = MakeButton(title, onClick);
            newButton.SetColor(color);
            return newButton;
        }
    }
}