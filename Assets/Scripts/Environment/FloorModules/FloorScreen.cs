using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SlimeBounce.Environment.FloorModules
{
    public abstract class FloorScreen : MonoBehaviour
    {
        [SerializeField] protected Animator screenAnimator;
        [SerializeField] protected AEContainer screenAnimEvents;
        [SerializeField] protected float actionDelay;
        protected Sequence _actionSequence;

        protected virtual void Awake()
        {
            screenAnimEvents.OnActionA += MenuAppeared;
            screenAnimEvents.OnActionB += HideFinished;
        }

        public virtual void Show()
        {
            if (!gameObject.activeSelf)
                _actionSequence = null;
            gameObject.SetActive(true);
            screenAnimator.SetBool("Shown", true);
        }

        public virtual void Hide()
        {
            screenAnimator.SetBool("Shown", false);
        }

        private void HideFinished()
        {
            gameObject.SetActive(false);
        }

        protected abstract void MenuAppeared();

        protected void DelayedResolution(Action resolvedAction, bool isFinalAction = true)
        {
            if (_actionSequence == null)
            {
                _actionSequence = DOTween.Sequence();
                _actionSequence.AppendInterval(actionDelay);
                _actionSequence.AppendCallback(() => resolvedAction.Invoke());
                if (isFinalAction)
                {
                    Hide();
                } 
                else
                {
                    _actionSequence.AppendCallback(() => _actionSequence = null);
                }
            }
        }
    }
}