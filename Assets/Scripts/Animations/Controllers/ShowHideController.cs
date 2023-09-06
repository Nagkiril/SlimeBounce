using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Animations.Modules;

namespace SlimeBounce.Animations.Controllers
{
    public abstract class ShowHideController : AnimationController
    {
        public bool IsShown { get; protected set; }

        protected override void Awake()
        {
            base.Awake();
            IsShown = true;
        }


        public abstract void Show(bool animate = true);

        public abstract void Hide(bool animate = true);
    }
}