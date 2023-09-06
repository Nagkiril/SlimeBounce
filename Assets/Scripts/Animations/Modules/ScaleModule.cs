using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SlimeBounce.Animations.Modules
{
    public class ScaleModule : VisualModule
    {
        [SerializeField] private Transform _targetTransform;
        [SerializeField] private CurveSet _activationCurves;
        [SerializeField] private CurveSet _deactivationCurves;

        private float _animationProgress;
        private CurveSet _animationSet;

        [Serializable]
        private class CurveSet
        {
            public AnimationCurve ScaleX;
            public AnimationCurve ScaleY;
            public AnimationCurve ScaleZ;
            public float TotalDuration;
        }

        protected override void Awake()
        {
            base.Awake();
            enabled = false;
        }

        private void Update()
        {
            _animationProgress += Time.deltaTime / _animationSet.TotalDuration;
            _targetTransform.localScale = GetFrameScale();
            if (_animationProgress >= 1f)
            {
                enabled = false;
                NotifyTransitionDone();
            }
        }

        private Vector3 GetFrameScale()
        {
            var scaleX = _animationSet.ScaleX.Evaluate(_animationProgress);
            var scaleY = _animationSet.ScaleY.Evaluate(_animationProgress);
            var scaleZ = _animationSet.ScaleZ.Evaluate(_animationProgress);
            return new Vector3(scaleX, scaleY, scaleZ);
        }

        private void SetActivation(bool animate, bool isActive)
        {
            _animationSet = (isActive ? _activationCurves : _deactivationCurves);
            if (animate)
            {
                _animationProgress = 0;
                enabled = true;
                _targetTransform.localScale = GetFrameScale();
            }
            else
            {
                _animationProgress = 1;
                _targetTransform.localScale = GetFrameScale();
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