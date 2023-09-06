using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SlimeBounce.Slime.Visuals.Face.Implementations.Sprites
{
    public class FaceSpritePiece : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer _targetRenderer;
        [SerializeField] protected float _disposeDuration = 0.4f;
        [SerializeField] protected float _disposeDelay;
        [SerializeField] protected float _appearDuration = 0.4f;
        protected Sequence _disposeSequence;
        protected Tween _appearTween;

        protected virtual void Awake()
        {
            if (_appearDuration > 0)
            {
                var fullScale = transform.localScale;
                transform.localScale = Vector3.zero;
                _appearTween = transform.DOScale(fullScale, _appearDuration);
            }
        }

        //Empty FixedUpdate is here just in case so we need to add something on it for every FaceSpritePiece, while child classes could utilize it as well without breaking anything
        protected virtual void FixedUpdate()
        {

        }

        protected virtual void SelfDestruct()
        {
            Destroy(gameObject);
        }

        public virtual void Mirror()
        {
            var newOffset = transform.localPosition;
            newOffset.x *= -1;
            transform.localPosition = newOffset;
            _targetRenderer.flipX = !_targetRenderer.flipX;
        }

        public virtual void Dispose()
        {
            if (_disposeSequence == null)
            {
                _appearTween.Kill();
                _disposeSequence = DOTween.Sequence();
                _disposeSequence.Insert(_disposeDelay, transform.DOScale(Vector3.zero, _disposeDuration));
                _disposeSequence.Insert(_disposeDelay, _targetRenderer.DOColor(Color.clear, _disposeDuration));
                _disposeSequence.AppendCallback(SelfDestruct);
            }
        }

        public virtual void Tint(Color newColor)
        {
            _targetRenderer.color = newColor;
        }
    }
}