using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SlimeBounce.Slime.Control.Indicator
{
    public class SlimeIndicatorVisual : MonoBehaviour
    {
        [SerializeField] private Animator _ownAnim;
        private Tween _moveTween;
        private bool _isPositionImpossible;


        public void MoveTo(Vector3 newPosition, float moveDuration = 0.1f)
        {
            if (_moveTween != null)
                _moveTween.Kill();
            _moveTween = transform.DOMove(newPosition, moveDuration);
            _moveTween.SetEase(Ease.Linear);
        }

        public void MarkImpossibleLocation(bool isImpossible)
        {
            if (isImpossible != _isPositionImpossible)
            {
                _moveTween.Complete();
                _isPositionImpossible = isImpossible;
                _ownAnim.SetBool("IsImpossible", isImpossible);
            }
        }

        public void Show()
        {
            _ownAnim.SetBool("IsShown", true);
        }

        public void Hide()
        {
            _ownAnim.SetBool("IsShown", false);
            MarkImpossibleLocation(false);
        }

        public void SetVisualProhibition(bool isProhibited)
        {
            _ownAnim.SetBool("IsProhibited", isProhibited);
        }

        public Vector3 GetPosition() => transform.position;
    }
}