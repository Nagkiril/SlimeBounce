using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SlimeBounce.Animations.Modules
{
    public class MaterialFloatModule : VisualModule
    {
        [SerializeField] private SkinnedMeshRenderer _targetRenderer;
        [SerializeField] private string _propertyName;
        [SerializeField] private float _activeValue;
        [SerializeField] private float _inactiveValue;
        [SerializeField] private float _transitionDuration;

        private Tween _animationTween;


        private void AnimateFloat(bool isActive)
        {
            if (_animationTween != null)
                _animationTween.Kill();
            var endValue = (isActive ? _activeValue : _inactiveValue);
            _animationTween = _targetRenderer.material.DOFloat(endValue, _propertyName, _transitionDuration);
            _animationTween.OnComplete(() => { NotifyTransitionDone(); });
        }

        private void SetActivation(bool animate, bool isActive)
        {
            if (animate)
                AnimateFloat(isActive);
            else
            {
                _targetRenderer.material.SetFloat(_propertyName, GetEndValue(isActive));
                NotifyTransitionDone();
            }
        }

        private float GetEndValue(bool isActive)
        {
            return (isActive ? _activeValue : _inactiveValue);
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