using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeBounce.Animations.Modules.UI
{
    public class CanvasGroupModule : VisualModule
    {
        [SerializeField] private CanvasGroup _targetGroup;
        [SerializeField] private CurveSet _activationCurves;
        [SerializeField] private CurveSet _deactivationCurves;

        private float _animationProgress;
        private CurveSet _animationSet;

        [Serializable]
        private class CurveSet
        {
            public AnimationCurve Alpha;
            public float TotalDuration;
        }

        protected override void Awake()
        {
            base.Awake();
            if (_animationSet == null)
                enabled = false;
        }

        private void Update()
        {
            _animationProgress += Time.deltaTime / _animationSet.TotalDuration;
            _targetGroup.alpha = GetFrameAlpha();
            if (_animationProgress >= 1f)
            {
                enabled = false;
                NotifyTransitionDone();
            }
        }

        private float GetFrameAlpha()
        {
            return _animationSet.Alpha.Evaluate(_animationProgress);
        }

        private void SetActivation(bool animate, bool isActive)
        {
            _animationSet = (isActive ? _activationCurves : _deactivationCurves);
            if (animate)
            {
                _animationProgress = 0;
                enabled = true;
            }
            else
            {
                _animationProgress = 1;
                _targetGroup.alpha = GetFrameAlpha();
                NotifyTransitionDone();
            }
        }

        public override void Activate(bool animate = true)
        {
            SetActivation(animate, true);
        }


        public override void Deactivate(bool animate = true)
        {
            SetActivation(animate, false);
        }
    }
}