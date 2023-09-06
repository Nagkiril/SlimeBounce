using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Animations.Modules;

namespace SlimeBounce.Animations.Controllers
{
    public abstract class AnimationController : MonoBehaviour
    {
        protected List<VisualModule> _animatedModules;

        public event Action OnAnimationFinish;
        public bool IsAnimating { get => _animatedModules.Count > 0; }

        protected virtual void Awake()
        {
            _animatedModules = new List<VisualModule>();
        }

        protected virtual void OnTransitionDone(VisualModule target)
        {
            if (IsAnimating)
            {
                _animatedModules.Remove(target);
                if (!IsAnimating)
                    NotifyAnimationFinish();
            }
        }

        protected virtual void ActivateSet(VisualModule[] targetModules, bool animate)
        {
            if (animate)
                _animatedModules.AddRange(targetModules);
            foreach (var module in targetModules)
            {
                module.Activate(animate);
            }
        }

        protected virtual void DeactivateSet(VisualModule[] targetModules, bool animate)
        {
            if (animate)
                _animatedModules.AddRange(targetModules);
            foreach (var module in targetModules)
            {
                module.Deactivate(animate);
            }
        }

        protected void NotifyAnimationFinish()
        {
            OnAnimationFinish?.Invoke();
        }
    }
}