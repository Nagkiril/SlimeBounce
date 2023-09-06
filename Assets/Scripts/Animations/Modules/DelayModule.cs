using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SlimeBounce.Animations.Modules
{
    public class DelayModule : VisualModule
    {
        [SerializeField] private VisualModule _targetModule;
        [SerializeField] private float _activateModuleDelay;
        [SerializeField] private float _deactivateModuleDelay;

        private bool _isInitialized;
        private Sequence _delaySequence;

        private void Initialize()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                _targetModule.OnTransitionComplete += OnTargetTransitionComplete;
            }
            if (_delaySequence != null)
                _delaySequence.Kill();
            _delaySequence = DOTween.Sequence();
        }

        private void OnTargetTransitionComplete(VisualModule target)
        {
            NotifyTransitionDone();
        }

        public override void Activate(bool animate = true)
        {
            Initialize();
            _delaySequence.AppendInterval(_activateModuleDelay);
            _delaySequence.AppendCallback(() => { _targetModule.Activate(animate); });
        }

        public override void Deactivate(bool animate = true)
        {
            Initialize();
            _delaySequence.AppendInterval(_deactivateModuleDelay);
            _delaySequence.AppendCallback(() => { _targetModule.Deactivate(animate); });
        }
    }
}