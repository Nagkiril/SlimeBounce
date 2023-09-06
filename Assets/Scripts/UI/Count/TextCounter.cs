using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace SlimeBounce.UI.Count
{
    public class TextCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _animatedText;
        private Tweener _count;
        private int _currentCount;

        public event Action OnAnimationDone;

        public void CountTo(int valueTo, float duration)
        {
            if (_count != null)
            {
                _count.Kill();
            }
            _count = DOTween.To(() => _currentCount, x => _currentCount = x, valueTo, duration);
            _count.OnUpdate(() => _animatedText.text = _currentCount.ToString() );
            _count.OnComplete(() => { _animatedText.text = valueTo.ToString(); OnAnimationDone?.Invoke(); });
        }
    }
}