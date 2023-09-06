using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Animations.Modules;

namespace SlimeBounce.Animations.Controllers
{
    public class ShowHideActiveController : ShowHideController
    {
        [SerializeField] private VisualModule[] _showModules;
        [SerializeField] private VisualModule[] _hideModules;

        protected override void Awake()
        {
            base.Awake();
            foreach (var module in _showModules)
            {
                module.OnTransitionComplete += OnTransitionDone;
            }
            foreach (var module in _hideModules)
            {
                module.OnTransitionComplete += OnTransitionDone;
            }
        }

        public override void Show(bool animate = true)
        {
            IsShown = true;
            ActivateSet(_showModules, animate);
        }

        public override void Hide(bool animate = true)
        {
            IsShown = false;
            ActivateSet(_hideModules, animate);
        }
    }
}