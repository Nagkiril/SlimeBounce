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
        [SerializeField] TextMeshProUGUI animatedText;

        public event Action OnAnimationDone;

        Tweener _count;

        int _currentCount;

        public void CountTo(int valueTo, float duration)
        {
            if (_count != null)
            {
                _count.Kill();
            }
            _count = DOTween.To(() => _currentCount, x => _currentCount = x, valueTo, duration);
            _count.OnUpdate(() => animatedText.text = _currentCount.ToString() );
            _count.OnComplete(() => { animatedText.text = valueTo.ToString(); OnAnimationDone?.Invoke(); });
        }



    }
}