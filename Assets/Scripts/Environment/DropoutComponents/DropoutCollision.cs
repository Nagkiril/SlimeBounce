using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Slime.Control;
using SlimeBounce.Slime;

namespace SlimeBounce.Environment.DropoutComponents
{
    public class DropoutCollision : MonoBehaviour
    {
        public event Action<SlimeCore> OnSlimeHover;
        public event Action<SlimeCore> OnSlimeHoverEnd;

        private void Awake()
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            var slimeCollision = other.GetComponent<SlimeCollision>();
            if (slimeCollision != null)
            {
                var slime = slimeCollision.Slime;
                if (slime.IsEngaged)
                {
                    OnSlimeHover.Invoke(slime);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var slimeCollision = other.GetComponent<SlimeCollision>();
            if (slimeCollision != null)
            {
                var slime = slimeCollision.Slime;
                if (slime.IsEngaged)
                {
                    OnSlimeHoverEnd.Invoke(slime);
                }
            }
        }

        public void SetLeftSided(bool isLeftSided)
        {
            if (!isLeftSided)
            {
                transform.localScale -= new Vector3(2f * transform.localScale.x, 0, 0);
            }
        }
    }
}