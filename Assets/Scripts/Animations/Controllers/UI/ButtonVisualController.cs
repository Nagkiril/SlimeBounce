using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Animations.Modules;

namespace SlimeBounce.Animations.Controllers.UI
{
    public class ButtonVisualController : AnimationController
    {
        [SerializeField] private VisualModule[] _interactableToggleModules;
        [SerializeField] private VisualModule[] _pressModules;

        protected override void Awake()
        {
            base.Awake();
            foreach (var module in _interactableToggleModules)
            {
                module.OnTransitionComplete += OnTransitionDone;
            }
            foreach (var module in _pressModules)
            {
                module.OnTransitionComplete += OnTransitionDone;
            }
        }

        public void SetInteractable(bool isInteractable, bool animate = true)
        {
            if (isInteractable)
            {
                ActivateSet(_interactableToggleModules, animate);
            }
            else
            {
                DeactivateSet(_interactableToggleModules, animate);
            }
        }

        public void Press()
        {
            ActivateSet(_pressModules, true);
        }

        public void Release()
        {
            DeactivateSet(_pressModules, true);
        }
    }
}