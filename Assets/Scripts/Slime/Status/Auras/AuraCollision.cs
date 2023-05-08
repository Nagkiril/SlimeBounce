using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Slime.Control;

namespace SlimeBounce.Slime.Status.AuraComponents
{
    public class AuraCollision : MonoBehaviour
    {
        public List<SlimeCore> AffectedSlimes { get; private set; }

        public event Action<SlimeCore> OnSlimeEnter;

        public event Action<SlimeCore> OnSlimeExit;


        private void Awake()
        {
            AffectedSlimes = new List<SlimeCore>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var slimeCollision = other.GetComponent<SlimeCollision>();
            if (slimeCollision != null && !AffectedSlimes.Contains(slimeCollision.Slime))
            {
                AffectedSlimes.Add(slimeCollision.Slime);
                OnSlimeEnter?.Invoke(slimeCollision.Slime);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var slimeCollision = other.GetComponent<SlimeCollision>();
            if (slimeCollision != null && AffectedSlimes.Contains(slimeCollision.Slime))
            {
                AffectedSlimes.Remove(slimeCollision.Slime);
                OnSlimeExit?.Invoke(slimeCollision.Slime);
            }
        }
    }
}