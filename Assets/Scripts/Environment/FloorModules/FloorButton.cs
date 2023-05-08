using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Clickable;

namespace SlimeBounce.Environment.FloorModules
{
    public class FloorButton : MonoBehaviour
    {
        [SerializeField] Animator ownAnimator;
        [SerializeField] ClickableCollider clickZone;
        [SerializeField] bool clickableOnlyActive;

        public event Action OnButtonClicked;

        private void Awake()
        {
            clickZone.OnClickStart += OnClickStart;
            clickZone.OnClickEnd += OnClickEnd;
        }

        public void SetButtonActivity(bool isActive)
        {
            ownAnimator.SetBool("Active", isActive);
            clickZone.SetClickable(isActive);
        }

        void OnClickStart()
        {
            if (!clickableOnlyActive || ownAnimator.GetBool("Active"))
                ownAnimator.SetBool("Clicked", true);
        }

        void OnClickEnd()
        {
            if (!clickableOnlyActive || ownAnimator.GetBool("Active"))
            {
                ownAnimator.SetBool("Clicked", false);
                OnButtonClicked?.Invoke();
            }
        }

    }
}