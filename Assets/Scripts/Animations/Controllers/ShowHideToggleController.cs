using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Animations.Modules;

namespace SlimeBounce.Animations.Controllers
{
    public class ShowHideToggleController : ShowHideController
    {
        [SerializeField] private VisualModule[] _targetModules;

        protected override void Awake()
        {
            base.Awake();
            foreach (var module in _targetModules)
            {
                module.OnTransitionComplete += OnTransitionDone;
            }
        }

        public override void Show(bool animate = true)
        {
            IsShown = true;
            ActivateSet(_targetModules, animate);
        }

        public override void Hide(bool animate = true)
        {
            IsShown = false;
            DeactivateSet(_targetModules, animate);
        }
    }
}