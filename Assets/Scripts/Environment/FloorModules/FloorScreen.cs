using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SlimeBounce.Environment.FloorModules
{
    public abstract class FloorScreen : MonoBehaviour
    {
        [SerializeField] protected Animator _screenAnimator;
        [SerializeField] protected AEContainer _screenAnimEvents;
        [SerializeField] protected float _actionDelay;
        protected Sequence _actionSequence;

        protected virtual void Awake()
        {
            _screenAnimEvents.OnActionA += MenuAppeared;
            _screenAnimEvents.OnActionB += HideFinished;
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
                _actionSequence.AppendInterval(_actionDelay);
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

        public virtual void Show()
        {
            if (!gameObject.activeSelf)
                _actionSequence = null;
            gameObject.SetActive(true);
            _screenAnimator.SetBool("Shown", true);
        }

        public virtual void Hide()
        {
            _screenAnimator.SetBool("Shown", false);
        }
    }
}