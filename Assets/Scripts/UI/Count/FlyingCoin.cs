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
        [SerializeField] Animator coinAnimator;
        [SerializeField] AEContainer animEvents;
        [SerializeField] RectTransform ownRect;
        [SerializeField] float flightTimeRandom;
        [SerializeField] float disperseToFlightRatio;
        [SerializeField] Vector3 dispersePositionMin;
        [SerializeField] Vector3 dispersePositionMax;


        public event Action<FlyingCoin> OnDisperseFinished;
        public event Action<FlyingCoin> OnFlightFinished;

        Sequence _flight;

        void Awake()
        {
            animEvents.OnAnimationDone += OnAnimationDone;
            if (disperseToFlightRatio >= 1 || disperseToFlightRatio <= 0)
                Debug.LogWarning($"DisperseToFlightRatio should be within (0,1)! Current value is {disperseToFlightRatio}");
            if (disperseToFlightRatio >= 1 || disperseToFlightRatio <= 0)
                Debug.LogWarning($"FlightTimeRandomization should be within (0,1)! Current value is {flightTimeRandom}");
        }

        private void Start()
        {
            if (_flight == null)
            {
                Debug.LogWarning($"FlyingCoin should have a flight assigned right after instantiation!");
            }
        }

        public void SetAnchor(Vector2 anchorPoint)
        {
            ownRect.anchoredPosition = anchorPoint;
        }

        public void FlyToLocal(Vector3 newLocalPosition, float duration = 2)
        {
            duration += UnityEngine.Random.Range(-flightTimeRandom, flightTimeRandom) * duration;
            coinAnimator.SetBool("Show", true);
            coinAnimator.SetBool("Disperse", true);
            var _dispersePosition = MathUtils.RandomVector3(transform.localPosition - dispersePositionMin, transform.localPosition + dispersePositionMax);
            _flight = DOTween.Sequence();
            _flight.Append(transform.DOLocalMove(_dispersePosition, duration * disperseToFlightRatio).SetEase(Ease.Linear));
            _flight.AppendCallback(OnDisperseDone);
            _flight.Append(transform.DOLocalMove(newLocalPosition, duration * (1 - disperseToFlightRatio)).SetEase(Ease.Linear));
            _flight.AppendCallback(OnFlightDone);
        }

        void OnDisperseDone()
        {
            if (gameObject != null)
            {
                coinAnimator.SetBool("Disperse", false);
                OnDisperseFinished?.Invoke(this);
            }
        }

        void OnFlightDone()
        {
            if (gameObject != null)
            {
                Disappear();
                OnFlightFinished?.Invoke(this);
            }
        }

        public void Disappear()
        {
            coinAnimator.SetBool("Show", false);
        }

        void OnAnimationDone()
        {
            Destroy(gameObject);
        }

    }
}