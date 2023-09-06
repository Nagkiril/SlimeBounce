using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SlimeBounce.Animations.Modules.UI
{
    public class RectSizeDeltaModule : VisualModule
    {
        [SerializeField] private RectTransform _targetRect;
        [SerializeField] private CurveSet _activationCurves;
        [SerializeField] private CurveSet _deactivationCurves;

        private float _animationProgress;
        private CurveSet _animationSet;

        [Serializable]
        private class CurveSet
        {
            public AnimationCurve DeltaX;
            public AnimationCurve DeltaY;
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
            _targetRect.sizeDelta = GetFrameSize();
            if (_animationProgress >= 1f)
            {
                enabled = false;
                NotifyTransitionDone();
            }
        }

        private Vector2 GetFrameSize()
        {
            return new Vector2(_animationSet.DeltaX.Evaluate(_animationProgress), _animationSet.DeltaY.Evaluate(_animationProgress));
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
                _targetRect.sizeDelta = GetFrameSize();
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