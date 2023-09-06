using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.Animations.Modules
{
    public abstract class VisualModule : MonoBehaviour
    {
        public event Action<VisualModule> OnTransitionComplete;

        protected virtual void Awake()
        {

        }

        protected void NotifyTransitionDone()
        {
            OnTransitionComplete?.Invoke(this);
        }

        public abstract void Activate(bool animate = true);

        public virtual void Deactivate(bool animate = true)
        {
            NotifyTransitionDone();
        }
    }
}