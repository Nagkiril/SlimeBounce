using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SlimeBounce.Slime;
using SlimeBounce.Slime.Control;

namespace SlimeBounce.Abilities.Components
{
    public class AbilityCollider : MonoBehaviour
    {
        public event Action<SlimeCore> OnSlimeEntry;
        public event Action<SlimeCore> OnSlimeExit;
        private List<SlimeCore> _slimesInCollider;

        private void Awake()
        {
            _slimesInCollider = new List<SlimeCore>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var enteredCollision = other.GetComponent<SlimeCollision>();
            if (enteredCollision != null && !_slimesInCollider.Contains(enteredCollision.Slime))
            {
                _slimesInCollider.Add(enteredCollision.Slime);
                enteredCollision.Slime.OnSlimeDestroyed += VerifySlimes;
                OnSlimeEntry?.Invoke(enteredCollision.Slime);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var leavingCollision = other.GetComponent<SlimeCollision>();
            if (leavingCollision != null && _slimesInCollider.Contains(leavingCollision.Slime))
            {
                _slimesInCollider.Remove(leavingCollision.Slime);
                leavingCollision.Slime.OnSlimeDestroyed -= VerifySlimes;
                OnSlimeExit?.Invoke(leavingCollision.Slime);
            }
        }

        private void VerifySlimes()
        {
            for (var i = _slimesInCollider.Count - 1; i >= 0; i--)
            {
                if (_slimesInCollider[i] == null || _slimesInCollider[i].gameObject == null)
                {
                    _slimesInCollider.RemoveAt(i);
                }
            }
        }

        public List<SlimeCore> GetInsideSlimes()
        {
            VerifySlimes();
            return _slimesInCollider;
        }
    }
}