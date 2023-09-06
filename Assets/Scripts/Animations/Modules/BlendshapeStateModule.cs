using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SlimeBounce.Animations.Modules
{
    public class BlendshapeStateModule : VisualModule
    {
        [SerializeField] private SkinnedMeshRenderer _targetRenderer;
        [SerializeField] private int _blendshapeIndex;
        [SerializeField] private float _activeValue;
        [SerializeField] private float _inactiveValue;
        [SerializeField] private float _transitionDuration;

        private Tween _animationTween;


        private void AnimateBlendshape(bool isActive)
        {
            if (_animationTween != null)
                _animationTween.Kill();
            _animationTween = DOTween.To(() => _targetRenderer.GetBlendShapeWeight(_blendshapeIndex), x => _targetRenderer.SetBlendShapeWeight(_blendshapeIndex, x), GetEndValue(isActive), _transitionDuration);
            _animationTween.OnComplete(() => { NotifyTransitionDone(); });
        }

        private void SetActivation(bool animate, bool isActive)
        {
            if (animate)
                AnimateBlendshape(isActive);
            else
            {
                _targetRenderer.SetBlendShapeWeight(_blendshapeIndex, GetEndValue(isActive));
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