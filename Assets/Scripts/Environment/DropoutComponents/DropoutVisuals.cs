using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SlimeBounce.Environment.DropoutComponents
{
    public class DropoutVisuals : MonoBehaviour
    {
        [SerializeField] private Animator _ownAnimator;
        [SerializeField] private AEContainer _animEvents;
        [SerializeField] private Transform _consumableContainer;
        [SerializeField] private float _consumptionCenterTime = 0.5f;
        public event Action OnConsumptionFinish;

        private void Awake()
        {
            _animEvents.OnAnimationDone += ConsumptionFinish;
        }

        private void TriggerConsumption()
        {
            _ownAnimator.SetTrigger("Consume");
        }

        private void ConsumptionFinish()
        {
            OnConsumptionFinish?.Invoke();
        }

        private void AttachConsumable(Transform consumable)
        {
            consumable.SetParent(_consumableContainer);
            consumable.DOLocalMove(Vector3.zero, _consumptionCenterTime);
        }

        public void SetHovered(bool isHovered)
        {
            _ownAnimator.SetBool("Hovered", isHovered);
        }

        public void SetAvailable(bool isReady)
        {
            _ownAnimator.SetBool("Available", isReady);
            if (!isReady)
                SetHovered(false);
        }

        public void ConsumeItem(Transform consumable)
        {
            AttachConsumable(consumable);
            TriggerConsumption();
        }
    }
}