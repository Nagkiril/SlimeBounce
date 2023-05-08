using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace SlimeBounce.Environment.DropoutComponents
{
    public class DropoutVisuals : MonoBehaviour
    {
        [SerializeField] Animator ownAnimator;
        [SerializeField] AEContainer animEvents;
        [SerializeField] Transform consumableContainer;
        [SerializeField] float consumptionCenterTime = 0.5f;
        public event Action OnConsumptionFinish;

        private void Awake()
        {
            animEvents.OnAnimationDone += ConsumptionFinish;
        }

        public void SetHovered(bool isHovered)
        {
            ownAnimator.SetBool("Hovered", isHovered);
        }

        public void SetAvailable(bool isReady)
        {
            ownAnimator.SetBool("Available", isReady);
            if (!isReady)
                SetHovered(false);
        }

        public void ConsumeItem(Transform consumable)
        {
            AttachConsumable(consumable);
            TriggerConsumption();
        }

        void TriggerConsumption()
        {
            ownAnimator.SetTrigger("Consume");
        }

        void ConsumptionFinish()
        {
            OnConsumptionFinish?.Invoke();
        }

        void AttachConsumable(Transform consumable)
        {
            consumable.SetParent(consumableContainer);
            consumable.DOLocalMove(Vector3.zero, consumptionCenterTime);
        }
    }
}