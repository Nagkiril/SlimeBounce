using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Clickable;

namespace SlimeBounce.Environment.FloorModules
{
    public class FloorButton : MonoBehaviour
    {
        //We should probably use tween aimation over Animators for these; realistically, it won't make much difference, but right now I don't think that Animators are warranted here
        [SerializeField] private Animator _ownAnimator;
        [SerializeField] private ClickableCollider _clickZone;
        [SerializeField] private bool _clickableOnlyActive;

        public event Action OnButtonClicked;

        //We could make a script that keeps such values (likely in a form of a dictionary, rather than pure statics), that would be much nicer
        private static int ACTIVE_HASH;
        private static int CLICKED_HASH;

        private void Awake()
        {
            _clickZone.OnClickStarted += OnClickStart;
            _clickZone.OnClickEnded += OnClickEnd;
            CalculateHashes();
        }

        private static void CalculateHashes()
        {
            if (ACTIVE_HASH == 0)
            {
                ACTIVE_HASH = Animator.StringToHash("Active");
                CLICKED_HASH = Animator.StringToHash("Clicked");
            }
        }
        
        private void OnClickStart()
        {
            if (!_clickableOnlyActive || _ownAnimator.GetBool(ACTIVE_HASH))
                _ownAnimator.SetBool(CLICKED_HASH, true);
        }

        private void OnClickEnd()
        {
            if (!_clickableOnlyActive || _ownAnimator.GetBool(ACTIVE_HASH))
            {
                _ownAnimator.SetBool(CLICKED_HASH, false);
                OnButtonClicked?.Invoke();
            }
        }

        public void SetButtonActivity(bool isActive)
        {
            _ownAnimator.SetBool(ACTIVE_HASH, isActive);
            _clickZone.SetClickable(isActive);
        }
    }
}