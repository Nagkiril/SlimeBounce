using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Utils;

namespace SlimeBounce.UI.Count
{
    public class FlyingCoin : MonoBehaviour
    {
        [SerializeField] private Animator _coinAnimator;
        [SerializeField] private AEContainer _animEvents;
        [SerializeField] private RectTransform _ownRect;
        [SerializeField] private float _flightTimeRandom;
        [SerializeField] private float _disperseToFlightRatio;
        [SerializeField] private Vector3 _dispersePositionMin;
        [SerializeField] private Vector3 _dispersePositionMax;
        private Sequence _flight;

        public event Action<FlyingCoin> OnDisperseFinished;
        public event Action<FlyingCoin> OnFlightFinished;

        private void Awake()
        {
            _animEvents.OnAnimationDone += OnAnimationDone;
            //Maybe we should just limit it in the inspector? Not sure
            if (_disperseToFlightRatio >= 1 || _disperseToFlightRatio <= 0)
                Debug.LogWarning($"DisperseToFlightRatio should be within (0,1)! Current value is {_disperseToFlightRatio}");
            if (_disperseToFlightRatio >= 1 || _disperseToFlightRatio <= 0)
                Debug.LogWarning($"FlightTimeRandomization should be within (0,1)! Current value is {_flightTimeRandom}");
        }

        private void Start()
        {
            if (_flight == null)
            {
                Debug.LogWarning($"FlyingCoin should have a flight assigned right after instantiation!");
            }
        }

        private void OnAnimationDone()
        {
            Destroy(gameObject);
        }

        private void OnDisperseDone()
        {
            if (gameObject != null)
            {
                _coinAnimator.SetBool("Disperse", false);
                OnDisperseFinished?.Invoke(this);
            }
        }

        private void OnFlightDone()
        {
            if (gameObject != null)
            {
                Disappear();
                OnFlightFinished?.Invoke(this);
            }
        }

        public void SetAnchor(Vector2 anchorPoint)
        {
            _ownRect.anchoredPosition = anchorPoint;
        }

        public void FlyToLocal(Vector3 newLocalPosition, float duration = 2)
        {
            duration += UnityEngine.Random.Range(-_flightTimeRandom, _flightTimeRandom) * duration;
            _coinAnimator.SetBool("Show", true);
            _coinAnimator.SetBool("Disperse", true);
            var _dispersePosition = MathUtils.RandomVector3(transform.localPosition - _dispersePositionMin, transform.localPosition + _dispersePositionMax);
            _flight = DOTween.Sequence();
            _flight.Append(transform.DOLocalMove(_dispersePosition, duration * _disperseToFlightRatio).SetEase(Ease.Linear));
            _flight.AppendCallback(OnDisperseDone);
            _flight.Append(transform.DOLocalMove(newLocalPosition, duration * (1 - _disperseToFlightRatio)).SetEase(Ease.Linear));
            _flight.AppendCallback(OnFlightDone);
        }

        public void Disappear()
        {
            _coinAnimator.SetBool("Show", false);
        }

    }
}