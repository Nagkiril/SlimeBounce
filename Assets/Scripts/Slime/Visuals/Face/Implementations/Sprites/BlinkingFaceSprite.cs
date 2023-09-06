using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SlimeBounce.Slime.Visuals.Face.Implementations.Sprites
{
    public class BlinkingFaceSprite : FaceSpritePiece, IBlinkHandler
    {
        [SerializeField] private Sprite _blinkSprite;
        [SerializeField] private float _blinkDuration;
        [SerializeField] private float _blinkDelay;
        private float _untilNextBlink;
        private Sequence _blinkSequence;


        private void Blink()
        {
            _untilNextBlink = _blinkDelay;
            if (_blinkSequence == null || !_blinkSequence.active)
            {
                var defaultSprite = _targetRenderer.sprite;
                _blinkSequence = DOTween.Sequence();
                _blinkSequence.Append(transform.DOScale(new Vector3(transform.localScale.x * 1.1f, 0, 1), 0.3f));
                _blinkSequence.AppendCallback(() => { ChangeSprite(_blinkSprite); });
                _blinkSequence.Append(transform.DOScale(transform.localScale, 0.2f));
                _blinkSequence.AppendInterval(_blinkDuration);
                _blinkSequence.Append(transform.DOScale(new Vector3(transform.localScale.x * 1.1f, 0, 1), 0.3f));
                _blinkSequence.AppendCallback(() => { ChangeSprite(defaultSprite); });
                _blinkSequence.Append(transform.DOScale(transform.localScale, 0.2f));
            }

        }

        private void ChangeSprite(Sprite newSprite)
        {
            _targetRenderer.sprite = newSprite;
        }

        override protected void FixedUpdate()
        {
            if (_untilNextBlink > 0)
            {
                _untilNextBlink -= Time.deltaTime;
                if (_untilNextBlink <= 0)
                {
                    Blink();
                }
            }
        }


        public override void Dispose()
        {
            _blinkSequence.Kill();
            _untilNextBlink = -1;
            base.Dispose();
        }

        public void AllowBlinking()
        {
            _untilNextBlink = _blinkDelay;
        }

    }
}
