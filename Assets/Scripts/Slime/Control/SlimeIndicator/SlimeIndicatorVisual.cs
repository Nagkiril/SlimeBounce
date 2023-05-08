using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SlimeBounce.Slime.Control.Indicator
{
    public class SlimeIndicatorVisual : MonoBehaviour
    {
        [SerializeField] Animator ownAnim;
        Tween _moveTween;
        bool _isPositionImpossible;



        // Start is called before the first frame update
        void Start()
        {
            
        }

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
                ownAnim.SetBool("IsImpossible", isImpossible);
            }
        }

        public void Show()
        {
            ownAnim.SetBool("IsShown", true);
        }

        public void Hide()
        {
            ownAnim.SetBool("IsShown", false);
            MarkImpossibleLocation(false);
        }

        public void SetVisualProhibition(bool isProhibited)
        {
            ownAnim.SetBool("IsProhibited", isProhibited);
        }

        public Vector3 GetPosition() => transform.position;
    }
}