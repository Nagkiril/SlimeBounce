using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Animations.Modules;

namespace SlimeBounce.Animations.Controllers
{
    public class AnimationTriggerController : AnimationController
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


        public void Trigger()
        {
            ActivateSet(_targetModules, true);
        }
    }
}