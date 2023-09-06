using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlimeBounce.Animations.Modules
{
    public class AnimatorStateModule : VisualModule
    {
        [SerializeField] private Animator _ownAnimator;
        [SerializeField] private string _stateName;

        private static Dictionary<string, int> _stateHashes;

        protected override void Awake()
        {
            if (_stateHashes == null)
                _stateHashes = new Dictionary<string, int>();
            if (!_stateHashes.ContainsKey(_stateName))
            {
                _stateHashes.Add(_stateName, Animator.StringToHash(_stateName));
            }
        }


        public override void Activate(bool animate = true)
        {
            _ownAnimator.SetBool(_stateHashes[_stateName], true);
            NotifyTransitionDone();
        }


        public override void Deactivate(bool animate = true)
        {
            _ownAnimator.SetBool(_stateHashes[_stateName], false);
            NotifyTransitionDone();
        }

    }
}